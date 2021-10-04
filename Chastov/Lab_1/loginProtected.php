<?php

define( 'DVWA_WEB_PAGE_TO_ROOT', '' );
define('link_auth', 'loginProtected.php');
require_once DVWA_WEB_PAGE_TO_ROOT . 'dvwa/includes/dvwaPage.inc.php';

dvwaPageStartup( array( 'phpids' ) );

dvwaDatabaseConnect();

if( isset( $_POST[ 'Login' ] ) ) {
	
	$antibot=false;
	$recaptcha = $_POST['g-recaptcha-response'];
         
        //Сразу проверяем, что он не пустой
        if(!empty($recaptcha)) 
        {
            //Получаем HTTP от recaptcha
            $recaptcha = $_REQUEST['g-recaptcha-response'];
            //Сюда пишем СЕКРЕТНЫЙ КЛЮЧ, который нам присвоил гугл
            $secret = '6LcGw2EaAAAAAOjhkpd4qoHkv-QdPkseMVipf_sG';
            //Формируем utl адрес для запроса на сервер гугла
            $url = "https://www.google.com/recaptcha/api/siteverify?secret=".$secret ."&response=".$recaptcha."&remoteip=".$_SERVER['REMOTE_ADDR'];
         
            //Инициализация и настройка запроса
            $curl = curl_init();
            curl_setopt($curl, CURLOPT_URL, $url);
            curl_setopt($curl, CURLOPT_RETURNTRANSFER, 1);
            curl_setopt($curl, CURLOPT_TIMEOUT, 10);
            curl_setopt($curl, CURLOPT_USERAGENT, "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16");
            //Выполняем запрос и получается ответ от сервера гугл
            $curlData = curl_exec($curl);
         
            curl_close($curl);  
            //Ответ приходит в виде json строки, декодируем ее
            $curlData = json_decode($curlData, true);
         
            //Смотрим на результат 
            if($curlData['success']) 
            {
                //Сюда попадем если капча пройдена, дальше выполняем обычные 
                //действия(добавляем коммент или отправляем письмо) с формой
                $antibot=true;
         
         
            } 
            else {$antibot=false;}
	
		}
	
	if ($antibot)
	{
		
		// Anti-CSRF
		if (array_key_exists ("session_token", $_SESSION)) {
			$session_token = $_SESSION[ 'session_token' ];
		} else {
			$session_token = "";
		}
		//echo "test".$session_token;
		checkToken( $_REQUEST[ 'user_token' ], $session_token, link_auth );
		
		$user = $_POST[ 'username' ];
		$user = stripslashes( $user );
		$user = ((isset($GLOBALS["___mysqli_ston"]) && is_object($GLOBALS["___mysqli_ston"])) ? mysqli_real_escape_string($GLOBALS["___mysqli_ston"],  $user ) : ((trigger_error("[MySQLConverterToo] Fix the mysql_escape_string() call! This code does not work.", E_USER_ERROR)) ? "" : ""));

		$pass = $_POST[ 'password' ];
		$pass = stripslashes( $pass );
		$pass = ((isset($GLOBALS["___mysqli_ston"]) && is_object($GLOBALS["___mysqli_ston"])) ? mysqli_real_escape_string($GLOBALS["___mysqli_ston"],  $pass ) : ((trigger_error("[MySQLConverterToo] Fix the mysql_escape_string() call! This code does not work.", E_USER_ERROR)) ? "" : ""));
		$pass = md5( $pass );

		$query = ("SELECT table_schema, table_name, create_time
					FROM information_schema.tables
					WHERE table_schema='{$_DVWA['db_database']}' AND table_name='users'
					LIMIT 1");
		$result = @mysqli_query($GLOBALS["___mysqli_ston"],  $query );
		if( mysqli_num_rows( $result ) != 1 ) {
			dvwaMessagePush( "First time using DVWA.<br />Need to run 'setup.php'." );
			dvwaRedirect( DVWA_WEB_PAGE_TO_ROOT . 'setup.php' );
		}

		$query  = "SELECT * FROM `users` WHERE user='$user' AND password='$pass';";
		$result = @mysqli_query($GLOBALS["___mysqli_ston"],  $query ) or die( '<pre>' . ((is_object($GLOBALS["___mysqli_ston"])) ? mysqli_error($GLOBALS["___mysqli_ston"]) : (($___mysqli_res = mysqli_connect_error()) ? $___mysqli_res : false)) . '.<br />Try <a href="setup.php">installing again</a>.</pre>' );
		if( $result && mysqli_num_rows( $result ) == 1 ) {    // Login Successful...
			dvwaMessagePush( "You have logged in as '{$user}'" );
			dvwaLogin( $user );
			dvwaRedirect( DVWA_WEB_PAGE_TO_ROOT . 'index.php' );
		}

		// Login failed
		dvwaMessagePush( 'Login failed' );
		dvwaRedirect( link_auth );
	}
	else
	{
		dvwaMessagePush('error captcha');
	}
	
}

$messagesHtml = messagesPopAllToHtml();

Header( 'Cache-Control: no-cache, must-revalidate');    // HTTP/1.1
Header( 'Content-Type: text/html;charset=utf-8' );      // TODO- proper XHTML headers...
Header( 'Expires: Tue, 23 Jun 2009 12:00:00 GMT' );     // Date in the past

// Anti-CSRF
generateSessionToken();

echo "<!DOCTYPE html>

<html lang=\"en-GB\">

	<script src='https://www.google.com/recaptcha/api.js'></script>
	
	<head>

		<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />

		<title>Login :: Damn Vulnerable Web Application (DVWA) v" . dvwaVersionGet() . "</title>

		<link rel=\"stylesheet\" type=\"text/css\" href=\"" . DVWA_WEB_PAGE_TO_ROOT . "dvwa/css/login.css\" />

	</head>

	<body>

	<div id=\"wrapper\">

	<div id=\"header\">

	<br />

	<p><img src=\"" . DVWA_WEB_PAGE_TO_ROOT . "dvwa/images/login_logo.png\" /></p>

	<br />

	</div> <!--<div id=\"header\">-->

	<div id=\"content\">

	<form action=\"".link_auth."\" method=\"post\">

	<fieldset>

			<label for=\"user\">Username</label> <input type=\"text\" class=\"loginInput\" size=\"20\" name=\"username\"><br />


			<label for=\"pass\">Password</label> <input type=\"password\" class=\"loginInput\" AUTOCOMPLETE=\"off\" size=\"20\" name=\"password\"><br />

			<br />

			<p class=\"submit\"><input type=\"submit\" value=\"Login\" name=\"Login\"></p>
			
			<div class='g-recaptcha' data-sitekey='6LcGw2EaAAAAAC3d_-DJOHWHTGvduDDqjAeAH-ED'></div></br>

	</fieldset>

	" . tokenField() . "

	</form>

	<br />

	{$messagesHtml}

	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />

	<!-- <img src=\"" . DVWA_WEB_PAGE_TO_ROOT . "dvwa/images/RandomStorm.png\" /> -->
	</div > <!--<div id=\"content\">-->

	<div id=\"footer\">

	<p>" . dvwaExternalLinkUrlGet( 'https://github.com/digininja/DVWA/', 'Damn Vulnerable Web Application (DVWA)' ) . "</p>

	</div> <!--<div id=\"footer\"> -->

	</div> <!--<div id=\"wrapper\"> -->

	</body>

</html>";

?>
