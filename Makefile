
all: x64d x64r x86d x86r 
	
x86d:
	msbuild WatchIT.sln /p:Configuration=Debug /p:Platform=x86
	
x86r:
	msbuild WatchIT.sln /p:Configuration=Release /p:Platform=x86
	
x64d:
	msbuild WatchIT.sln /p:Configuration=Debug /p:Platform=x64

x64r:
	msbuild WatchIT.sln /p:Configuration=Release /p:Platform=x64
	
x86dc:
	msbuild WatchIT.sln /p:Configuration=Debug /p:Platform=x86 /target:Clean
	
x86rc:
	msbuild WatchIT.sln /p:Configuration=Release /p:Platform=x86 /target:Clean

x64dc:
	msbuild WatchIT.sln /p:Configuration=Debug /p:Platform=x64 /target:Clean

x64rc:
	msbuild WatchIT.sln /p:Configuration=Release /p:Platform=x64 /target:Clean
	
clean: x86dc x86rc x64dc x64rc