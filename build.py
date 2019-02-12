import os
import subprocess
import sys
import shutil

cmd_ref = [
    ["clean", 0],
    ["publish", 0],
    ["content", 0],
    ["pack", 0],
    ["icon", 0],
    ["zip", 0]
    ]

def main():
    if (len(sys.argv) != 2):
        showUsage()
    cmd = sys.argv[1]
    found = 0
    for n in range(0, len(cmd_ref)):
        #print(cmd_ref[n][0])
        if (cmd == "all" or cmd == cmd_ref[n][0]):
            cmd_ref[n][1] = 1
            found = 1
    if (found == 0):
        showUsage()

    root = os.getcwd()
    changeDir("Source")

    #changeDir("Content")
    #for f in os.walk('.'):
    #    print(f)
    #changeDir(root)
    #changeDir("Source")

    #clean
    if (cmd_ref[0][1] == 1):
        runProcess("dotnet clean")

    #publish
    if (cmd_ref[1][1] == 1):
        if (os.path.isdir("Game/bin/Release/netcoreapp2.1/win-x64")):
            shutil.rmtree("Game/bin/Release/netcoreapp2.1/win-x64")
        runProcess("dotnet publish -r win-x64 -c release /p:TrimUnusedDependencies=true")

    #content
    if (cmd_ref[2][1] == 1):
        checkDir("Content")
        checkDir("Game/bin/Release/netcoreapp2.1/win-x64/publish")
        if (os.path.isdir("Game/bin/Release/netcoreapp2.1/win-x64/publish/Content")):
            shutil.rmtree("Game/bin/Release/netcoreapp2.1/win-x64/publish/Content")
        shutil.copytree("Content", "Game/bin/Release/netcoreapp2.1/win-x64/publish/Content")

    #pack
    if (cmd_ref[3][1] == 1):
        print()
        print("Not implemented!")

    #icon
    if (cmd_ref[4][1] == 1):
        print()
        print("Not implemented!")

    #zip
    if (cmd_ref[5][1] == 1):
        print()
        print("Not implemented!")

    print()
    print("Success!")

def showUsage():
        print("Usage: python build.py [command]")
        print("Commands:")
        print("    clean   Clean the project.")
        print("    publish Build release version.")
        print("    content Copy content to release folder.")
        print("    pack    Pack dlls into subdirectories.")
        print("    icon    Inject program icon.")
        print("    zip     Send release build to archive.")
        print("    all     Run all commands.")
        exit()

def changeDir(str):
    try:
        os.chdir(str)
    except:
        print()
        print("Folder '" + str + "' not found.")
        print()
        print("Process failed with errors.")
        quit()

def checkDir(str):
    if (os.path.isdir(str) == False):
        print()
        print("Directory '" + str + "' does not exist.")
        print()
        print("Process failed with errors.")
        quit()

def runProcess(str):
    print()
    print("Execute '" + str + "'")
    result = subprocess.run(str, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    print()
    print("stdout=")
    print(result.stdout)
    if (result.returncode != 0):
        print("Command failed!")
        print()
        print("stderr=")
        print(result.stderr)
        print()
        print("Process failed with errors.")
        quit()
    return result.returncode

if __name__ == '__main__':
    main()
