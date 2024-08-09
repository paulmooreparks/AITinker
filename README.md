# quallm
Query Any LLM - A CLI application for LLM interactions.

## Overview
Quallm (rhymes with "Gollum") is a CLI application for interacting with LLM (Language Model) APIs. It is implemented in .NET 8.0 and runs on Windows, Linux, and macOS.

Right now, Quallm only talks to ChatGPT, but in the future it will support multiple LLMs through plug-in libraries.

### Usage
Quallm may be used as traditional command-line tool, or as an interactive shell. The interactive shell is the default mode of operation.

```shell
$ quallm send "Are we live?"
Yes, we are live! How can I assist you today?

$ cat .\hello.bas | quallm send "Rewrite this program to ask for a name: "
You can modify the program to prompt the user for their name and then greet 
them accordingly. Here is a revised version of your program:

10 input "What is your name? "; name$
20 print "Hello, "; name$; "!"
30 end

In this version, the program asks for the user's name and then prints a 
personalized greeting.

$quallm
quallm: Query Any LLM v0.1.0.0
exit     Exit the application
help, ?  Show help and usage information

quallm> send "What is the capital of France?"
The capital of France is Paris.
```

### Configuration

To use quallm, you must have an API key for the LLM you wish to use. You can obtain an API key for ChatGPT from OpenAI.

Once you have your API key, create a file named `appsettings.json` in the `.quallm` directory in your home directory. The file should contain the following JSON:

```json
{
    "ChatGPT": {
        "ApiKey": "your-api-key-here",
        "ApiUrl": "https://api.openai.com/v1/chat/completions"
    }
}
```
