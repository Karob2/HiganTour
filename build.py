import os
import subprocess
import sys
import shutil
import json

cmd_ref = [
    ["clean", 0],
    ["publish", 0],
    ["content", 0],
    ["pack", 0],
    ["icon", 0]
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

    #clean
    if (cmd_ref[0][1] == 1):
        print()
        print(">>> Cleaning source.")
        print()
        runProcess("dotnet clean Source/")

    #publish
    if (cmd_ref[1][1] == 1):
        print()
        print(">>> Building self-contained project.")
        print()
        removeDir("publish-win-x64")
        removeDir("Source/Game/publish")
        runProcess("dotnet publish -r win-x64 -c release -o publish Source/")
        shutil.move("Source/Game/publish", "publish-win-x64")

    #content
    if (cmd_ref[2][1] == 1):
        print()
        print(">>> Copying resources to Content folder.")
        print()
        checkDir("Source/Content")
        checkDir("publish-win-x64")
        removeDir("publish-win-x64/Content")
        shutil.copytree("Source/Content", "publish-win-x64/Content")

    #pack
    if (cmd_ref[3][1] == 1):
        print()
        print(">>> Packing DLLs into lib directory.")
        print()
        changeDir("publish-win-x64")
        with open("Game.runtimeconfig.json", 'w') as file:
            file.write("{\"runtimeOptions\":{\"additionalProbingPaths\":[\"lib\"]}}")
        checkFile("Game.deps.json")
        with open("Game.deps.json") as f:
            data = json.load(f)
        for keyTarget in data["targets"]:
            dataTarget = data["targets"][keyTarget]
            for keyGroup in dataTarget:
                for keySubgroup in dataTarget[keyGroup]:
                    if keySubgroup != "runtime" and keySubgroup != "native": continue
                    for keyItem in dataTarget[keyGroup][keySubgroup]:
                        skip = False
                        filename = splitFilename(keyItem)
                        #print("......<" + filename[0] + "> " + filename[1] + " <" + filename[2] + ">")
                        if filename[2] != ".dll" and filename[2] != ".so" and filename[2] != ".dylib":
                            skip = True
                        if filename[1] == "Game" or filename[1] == "hostfxr" or filename[1] == "libhostfxr" or filename[1] == "hostpolicy" or filename[1] == "libhostpolicy":
                            skip = True
                        if skip == True:
                            print("Skipping " + keyItem)
                            continue
                        targetDir = os.path.join("lib", keyGroup.lower(), filename[0])
                        targetFile = os.path.join("lib", keyGroup.lower(), keyItem)
                        sourceFile = filename[1] + filename[2]
                        makeDir(targetDir)
                        shutil.move(sourceFile, targetFile)
        changeDir(root)

    #icon
    if (cmd_ref[4][1] == 1):
        print()
        print(">>> Injecting icon into executable.")
        print()
        print(sys.platform)
        if sys.platform != "win32":
            print()
            print("Not implemented on your platform! Skipping...")
        else:
            # since rcedit bungles file ownership, make a copy of the exe to restore default ownership properties
            runProcess("Tools/rcedit-x64.exe publish-win-x64/Game.exe --set-icon Source/Game/Icon.ico")
            os.rename("publish-win-x64/Game.exe", "publish-win-x64/_Game.exe")
            shutil.copy2("publish-win-x64/_Game.exe", "publish-win-x64/Game.exe")
            os.remove("publish-win-x64/_Game.exe")

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
        print("    all     Run all commands.")
        exit()

def changeDir(str):
    try:
        os.chdir(str)
    except:
        print("Folder '" + str + "' not found.")
        print()
        print("Process failed with errors.")
        quit()

def checkDir(str):
    if (os.path.isdir(str) == False):
        print("Directory '" + str + "' does not exist.")
        print()
        print("Process failed with errors.")
        quit()

def makeDir(str):
    if (os.path.isdir(str) == False):
        os.makedirs(str)

def removeDir(str):
    if (os.path.isdir(str)):
        shutil.rmtree(str)

def checkFile(str):
    if (os.path.isfile(str) == False):
        print("File '" + str + "' does not exist.")
        print()
        print("Process failed with errors.")
        quit()

def splitFilename(str):
    nExt = 0
    nPath = 0
    splitname = [0, 0, 0]
    for i in range(0, len(str)):
        if str[i] == ".":
            nExt = i
        if str[i] == "/" or str[i] == "\\":
            nPath = i + 1
    if nExt <= nPath:
        splitname[0] = str[:nPath]
        splitname[1] = str[nPath:]
        splitname[2] = ""
        return splitname

    splitname[0] = str[:nPath]
    splitname[1] = str[nPath:nExt]
    splitname[2] = str[nExt:]
    return splitname

def runProcess(str):
    print("Execute '" + str + "'")
    result = subprocess.run(str, stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    print()
    print("stdout=")
    #print(result.stdout)
    print(result.stdout.decode("unicode_escape"))
    if (result.returncode != 0):
        print("Command failed!")
        print()
        print("stderr=")
        #print(result.stderr)
        print(result.stderr.decode("unicode_escape"))
        print()
        print("Process failed with errors.")
        quit()
    return result.returncode

if __name__ == '__main__':
    main()
