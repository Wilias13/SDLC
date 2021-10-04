<?php  
if( isset( $_POST[ 'Login' ] ) && isset ($_POST['username']) && isset ($_POST['password']) ) {  
    checkToken( $_REQUEST[ 'user_token' ], $_SESSION[ 'session_token' ], 'index.php' );  
 
    $user = $_POST[ 'username' ];  
    $user = stripslashes( $user );  
    $user = ((isset($GLOBALS["___mysqli_ston"]) && is_object($GLOBALS["___mysqli_ston"])) ? mysqli_real_escape_string($GLOBALS["___mysqli_ston"],  $user ) : ((trigger_error("[MySQLConverterToo] Fix the mysql_escape_string() call! This code does not work.", E_USER_ERROR)) ? "" : ""));  
 
    $pass = $_POST[ 'password' ];  
    $pass = stripslashes( $pass );  
    $pass = ((isset($GLOBALS["___mysqli_ston"]) && is_object($GLOBALS["___mysqli_ston"])) ? mysqli_real_escape_string($GLOBALS["___mysqli_ston"],  $pass ) : ((trigger_error("[MySQLConverterToo] Fix the mysql_escape_string() call! This code does not work.", E_USER_ERROR)) ? "" : ""));  
    $pass = md5( $pass );  
 
    $total_failed_login = 3;  
    $lockout_time       = 15;  
    $account_locked     = false;  
 
    $data = $db->prepare( 'SELECT failed_login, last_login FROM users WHERE user = (:user) LIMIT 1;' );  
    $data->bindParam( ':user', $user, PDO::PARAM_STR );  
    $data->execute();  
    $row = $data->fetch();  
 
    if( ( $data->rowCount() == 1 ) && ( $row[ 'failed_login' ] >= $total_failed_login ) )  {  
	
        $last_login = strtotime( $row[ 'last_login' ] );  
        $timeout    = $last_login + ($lockout_time * 60);  
        $timenow    = time();  

        if( $timenow < $timeout ) {  
            $account_locked = true;  
        }  
    }  

    $data = $db->prepare( 'SELECT * FROM users WHERE user = (:user) AND password = (:password) LIMIT 1;' );  
    $data->bindParam( ':user', $user, PDO::PARAM_STR);  
    $data->bindParam( ':password', $pass, PDO::PARAM_STR );  
    $data->execute();  
    $row = $data->fetch();  
 
    if( ( $data->rowCount() == 1 ) && ( $account_locked == false ) ) {  
        $avatar       = $row[ 'avatar' ];  
        $failed_login = $row[ 'failed_login' ];  
        $last_login   = $row[ 'last_login' ];  

        echo "<p>Welcome to the password protected area <em>{$user}</em></p>";  
        echo "<img src=\"{$avatar}\" />";  
 
        if( $failed_login >= $total_failed_login ) {  
            echo "<p><em>Warning</em>: Someone might of been brute forcing your account.</p>";  
            echo "<p>Number of login attempts: <em>{$failed_login}</em>.<br />Last login attempt was at: <em>${last_login}</em>.</p>";  
        }  
 
        $data = $db->prepare( 'UPDATE users SET failed_login = "0" WHERE user = (:user) LIMIT 1;' );  
        $data->bindParam( ':user', $user, PDO::PARAM_STR );  
        $data->execute();  
    } else {  
        sleep( rand( 2, 4 ) );  

        echo "<pre><br />Username and/or password incorrect.<br /><br/>Alternative, the account has been locked because of too many failed logins.<br />If this is the case, <em>please try again in {$lockout_time} minutes</em>.</pre>";  

        $data = $db->prepare( 'UPDATE users SET failed_login = (failed_login + 1) WHERE user = (:user) LIMIT 1;' );  
        $data->bindParam( ':user', $user, PDO::PARAM_STR );  
        $data->execute();  
    }  
 
    $data = $db->prepare( 'UPDATE users SET last_login = now() WHERE user = (:user) LIMIT 1;' );  
    $data->bindParam( ':user', $user, PDO::PARAM_STR );  
    $data->execute();  
}  

generateSessionToken();  
 
?> 