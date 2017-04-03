@echo off
if not exist ("%cd%\afpant-folgebrev-autogen") (
	mkdir "afpant-folgebrev-autogen"
)
del /y "afpant-folgebrev-autogen\*"

"%programfiles(x86)%\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" "%CD%\afpant-folgebrev-xsd\afpant-folgebrev-1.0.0.xsd" /classes /language:C# /outputdir:"%CD%\afpant-folgebrev-autogen"

pause