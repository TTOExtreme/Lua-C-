--This is the bios for the lua interpreter made by TTOExtreme

--initialize Global Variables

Global Author = TTOExtreme;
Global Version = 1.3;

Global CurrentLocation = \_root\home\;

print(initializing);
--do the boot stuff
if(exist \_root\boot\boot.lua)then
	print(boot file found);
	start(\_root\boot\boot.lua);
else
	print([failure] boot file not found try to reinstall the system using Install);
end
