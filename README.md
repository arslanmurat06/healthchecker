# Health Checker

Web application to monitor target applications’ health. It takes a URL as input and
periodically checks whether it’s up or not. It sends a notification message when a request to the URL
returns a response code other than 2XX.

Created by using Asp NET MVC Core 6.

> For Authentication Scaffold Identity for Asp NET Core has been used.

## Tech Stack:
- Asp .Net Core Identity
- EntityFramework Core
- HangFire (schedule job)
- MailKit (mail)

## Test:
- XUnit (Test Framework)
- AutoFixture (Mock for dependeny injection)
- Moq
- EntityFramework Core In Memory
- HangFire InMemory

## TODOs:
- [ ] Add Rabbit MQ to queue the mails
- [ ] HangFire dashboard added to see the recurring jobs. Will be removed later, admins will only be authorized to see
- [ ] Publish Azure :tada:

## Preview:
![preview](https://github.com/arslanmurat06/healthchecker/blob/master/2021-12-02-01-23-25_1.gif)


