
### Running from Command Line

Build with 
`docker build -t questionapp -f QuestionApp/Dockerfile .`

and run with
`docker run -it --rm -p 4321:80 questionapp`
 
Accessible via `http://localhost:4321/swagger`