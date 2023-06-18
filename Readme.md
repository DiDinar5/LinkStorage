![](https://img.shields.io/aur/last-modified/google-chrome?logo=red&logoColor=green&style=plastic)
# LinkStorage :closed_lock_with_key:

## Описание
Веб-приложение разработано по шаблону ASP.NET Core Web API. Приложение предназначено для хранения адресов смарт-контрактов и взаимодействий с ними. В основе приложения лежит принцип слабосвязанной архитектуры, которая упрощает добавление новой функциональности.
____
## Начало работы
Пользователю, желающему воспользоваться платформой, необходимо пройти регистрацию и авторизацию, что позволит ему сохранять и удалять свои адреса смарт-контрактов. Для этого нужно воспользоваться методами контроллера, представленными ниже: 
![image](https://github.com/DiDinar5/LinkStorage/assets/106435950/4cc61dd6-c50a-4f39-88ec-bf512982cabc)


После того, как пользователь авторизируется, он получает уникальный JWT токен.
![image](https://github.com/DiDinar5/LinkStorage/assets/106435950/ea255dda-d133-4c5a-b021-64cfc2a72b05)
С данным токеном у пользователя в доступе только тот функционал, который соответствует его роли (Role:admin/user). 
Для авторизации, необходимо нажать на кнопку `Authorize`:unlock: и ввести "Bearer" и через пробел ваш токен, например "Bearer 1234qwerty". 
![image](https://github.com/DiDinar5/LinkStorage/assets/106435950/e38f44f7-bd6b-4744-8f5f-d4390c90cbb2)

Готово :ballot_box_with_check: Доступ к методам контроллера открыт, теперь функции в вашем распоряжении.
### Добавление адреса смарт-контракта :scroll: :link:
![image](https://github.com/DiDinar5/LinkStorage/assets/106435950/1bed3415-a3ff-4778-abf9-2a056468e789)

Как можно заметить, фиксируется дата :date: и присваивается идентификатор :id: , для удобного манипулирования адресом.
### Итоги
Маленькая часть API готова :white_check_mark:. Как было сказано выше, это приложение не боится добавления новых функций, а также взаимодействий с другими API. В скором времени добавится интерфейс(возможно с помощью React.js) и добавление взаимодействия с блокчейном(возможно создание отдельного API с помощью Node.js на примере [этого](https://blog.logrocket.com/interacting-smart-contracts-via-nodejs-api/)) :soon:.
#### Спасибо за проявленный интерес! Приятного пользования!


### Информация
По всем ошибкам и замечаниям прошу писать на почту :mailbox: <khayrutdinov.dd@gmail.com>, буду рад разобраться.
