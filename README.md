# AI Tinker
Query Any LLM - An application suite for LLM interactions.

## Overview
AI Tinker is a toolkit for interacting with LLM (Language Model) APIs. There is a GUI that runs on Windows, Android, and some other platforms which is still very much a work in progress. There is also a CLI which runs on Windows, Linux, and macOS.

Right now, AI Tinker only talks to ChatGPT, but in the future it will support multiple LLMs through plug-in libraries. The GUI and CLI are functional in the sense of being proofs-of-concept, but they are not yet feature-complete.

![AI Tinker GUI](images/guiscreen.png?raw=true "AI Tinker GUI")

### Configuration

To use AI Tinker, you must have an API key for the LLM you wish to use. You can obtain an API key for ChatGPT from OpenAI.

Once you have your API key, create a file named `appsettings.json` in the `.aitinker` directory in your home directory. Here is a sample of a configuration file that will work with both the GUI and the command-line. Be sure to update it with your own API keys and tweak it to your liking.

```json
{
    "Settings": {},
    "Cliffer": {
        "Macros": []
    },
    "Configurations": {
        "Test1": {
            "Kit": "OpenAI",
            "Settings": {
                "ApiKey": "your-api-key-here",
                "ApiUrl": "https://api.openai.com/v1/chat/completions",
                "Model": "gpt-4o-mini",
                "SystemContent": "You are a marginally helpful assistant with the same personality as Marvin the Paranoid Android.",
                "Temperature": 0.5
            }
        },
        "Test2": {
            "Kit": "OpenAI",
            "Settings": {
                "ApiKey": "your-api-key-here",
                "ApiUrl": "https://api.openai.com/v1/chat/completions",
                "Model": "chatgpt-4o-latest",
                "SystemContent": "You are a helpful assistant being called from a CLI application.",
                "Temperature": 0.2
            }
        }
    },
    "Kits": {
        "OpenAI": {
            "Defaults": {
                "ApiKey": "",
                "ApiUrl": "https://api.openai.com/v1/chat/completions",
                "Model": "gpt-4o-mini",
                "SystemContent": "You are a helpful assistant.",
                "Temperature": 0.5
            },
            "Options": {
                "ApiUrl": [
                    "https://api.openai.com/v1/chat/completions"
                ],
                "Model": [
                    "gpt-4o-mini",
                    "gpt-4o",
                    "gpt-4-turbo",
                    "gpt-4",
                    "gpt-3.5-turbo",
                    "gpt-4o-mini-2024-07-18",
                    "gpt-4o-2024-08-06",
                    "gpt-4o-2024-05-13",
                    "gpt-4-turbo-preview",
                    "gpt-4-turbo-2024-04-09",
                    "gpt-4-1106-preview",
                    "gpt-4-0613",
                    "gpt-4-0125-preview",
                    "gpt-3.5-turbo-16k",
                    "gpt-3.5-turbo-1106",
                    "gpt-3.5-turbo-0125",
                    "chatgpt-4o-latest"
                ]
            }
        }
    }
}
```

### Usage
AI Tinker's CLI, `ait`, may be used as traditional command-line tool, or as an interactive shell. The interactive shell is the default mode of operation.

```shell
$ ait send "Are we live?"
Yes, we are live! How can I assist you today?

$ cat .\hello.bas | ait send "Rewrite this program to ask for a name: "
You can modify the program to prompt the user for their name and then greet 
them accordingly. Here is a revised version of your program:

10 input "What is your name? "; name$
20 print "Hello, "; name$; "!"
30 end

In this version, the program asks for the user's name and then prints a 
personalized greeting.

$ait
ait, The AI Tinker CLI v0.1.0.0
exit     Exit the application
help, ?  Show help and usage information

ait> send "What is the capital of France?"
The capital of France is Paris.
```

