# BackendAT
Backend Advanced Test


A estrutura do projeto ficou igual aos projetos que estou habituado a trabalhar.
Deixei exemplos de como seria toda a aplicação completa (WeatherController), incluindo banco de dados e validações, no qual o fluxo seria:
Requisicao GET/POST/PUT/DELETE -> Controller -> CommandHandler -> Commands(validação) -> Repository(gravação);

O fluxo para obter as listas de musicas pela cidade/latitude/longitude seria: 
1. A requisição chega no Controller (GET com city ou lat/lon)
2. O Controller chama o CommandHandler que gerencia o que deve ser feito (buscar a temperatura por cidade ou latitude e longitude e com ela obter as musicas de acordo com o genero)
3. Para garantir o funcionamento, sempre estou gerando um novo token do spotify para o app backend em cada requisicao para a api (os dados estão no appsettings.js da pasta Backend.Api)
4. O CommandHandler retorna o resultado da requisição api (lista de strings com o nome das musicas)
ps. Para previnir erros, caso não seja encontrado a cidade ou latitude/longitude ele retorna a temperatura zero.

A aplicação contém um docker file bastando criar a imagem e depois rodar o container.
Os comandos abaixos foram rodados:
1. Dentro de Backend.Api rodar dotnet publish -o ./dist para criar publicar
2. Criado o .Dockerfile
3. Rodado o build: docker build -t aspnetbackend:1.0 .
4. Verificado a imagem criada: docker images
5. Rodado o container na porta 5000: docker container run -p 5000:80 aspnetbackend:1.0
