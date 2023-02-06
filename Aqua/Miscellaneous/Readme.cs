namespace Aqua.Miscellaneous
{
    public class Readme
    {
        public static string readMeString = @"------------------------------------------------------------------------------------

Welcome to the open-source world of Aqua System [by iplux].

Congratulations on installing Aqua System version 0.2.1 !
This system aims to be a different way for computing,
while still being simple to use and to understand.

This release is probably the biggest release so far,
in terms of bug fixing, optimizations and features.

This readme was created on : 31/01/2023.
Updated on : 04/02/2023.

------------------------------------------------------------------------------------

DISCLAMER : 

Please do not boot this system up from a real computer,
it could severly corrupt the computer drives due to a COSMOS filesystem
malfunction !

------------------------------------------------------------------------------------

IF YOU ARE UPDATING FROM 0.2.0 TO 0.2.1, INNER WORKINGS HAVE CHANGED : 

If you had a directory called ""System"" at the root of your drive, it contains
your old Aqua System installation, if you want to keep your old installation,
simply move the files from System to the AquaSys folder, else, you can
just delete it.

------------------------------------------------------------------------------------

Check out the ""man"" command [not fully implemented yet] to know how 
this system works, and also check the available 
commands and their counterpart in other operating systems.

------------------------------------------------------------------------------------

You're probably reading this text from either :

A - The fresh new TED Editor (still in Alpha).
B - The soon-to-be deprecated ""f read"" executable.
C - An external system.

------------------------------------------------------------------------------------

If you are reading from TED Editor : 
Welcome to the brand new TED Editor (based on Looti on Github) !

If not, you can always access TED using the ""ted"" + the file you want to edit.

------------------------------------------------------------------------------------

Bugfixes :

0.2.1 :
- Fixed a small bug where some dialogs were unspaced.
- Fixed a bug where the setup tried to format, without success.

0.2.0 :
- Finally fixed a bug with directory creating, and deleting.
- Fixed a bug with file creating.
- Fixed a bug with the canvas creating.
- Fixed the clear function not clearing all the way.
- Fixed a bug with sounds.
- Fixed a bug where the account sometimes did not create.
- Fixed a bug where the first time setup would not launch.
- Fixed some fatal bugs.

------------------------------------------------------------------------------------

Additions :

0.2.1 :
- An option to re-enter your username after 3 password attempts.
- An in-development TED Editor complete rewrite.
- A brand new Aqua Rescue Disk.

0.2.0 :
- A brand new text editor [TED Editor].
- A new package manager based on Verde, still in the works [""pm""].
- A manual [""man""].
- You can now choose if you want to be root, or not.
- Root permissions are finally useful.
- New commands [""time"", ""ted"", and ""calc""].
- New ""guest"" account, which restricts the user but does not require a password.
- New graphical interface [still in the works] !
- This little readme file.
- A new terminal font and bigger terminal resolution.
- Primitive support for themes in the TED Editor.
- New libraries in the code.
- Modified the crash logs to include the date and time.
- New filesystem utility [""format""].
- Code reformatting (File commands being in the same files, same for Network).
- Finally being able to make a file or directory with spaces !

------------------------------------------------------------------------------------

Bugs to fix :

- Slow scrolling due to the bar.

------------------------------------------------------------------------------------

Future features :

- Graphic File Manager on the terminal.
- Chatting software.
- Implement tabs for multiple terminal instances.
- External Executables [""elf"", and a custom-made ""ape""* format].
- Internet Connectivity commands [like ""weather"", ""get"", or ""update""].
- ACF files finally being useful : text coloring using tags, fonts, etc...
- Package Manager revamp.
- Games [Snake, Pong, Doom, etc...]
- Perhaps more..? ;]

* : Aqua Power Executable, ""Aqua Power"" being a program framework.

------------------------------------------------------------------------------------

1st of Feburary ""Blog"" post :

I'm really happy to see where this update is going.

When I created Aqua System, at first, it was just to experiment
with the Cosmos OS C# Framework, but then it became more than
just a hobby.

It became a full-fledged operating system.
Thank you for supporting Aqua System. :]

------------------------------------------------------------------------------------

Thank you for using Aqua System version 0.2.1 !

I am incredibly grateful for your support in using Aqua.
Please report any bugs you see on the official Aqua System Github** repository.

I hope you continue to enjoy using it,
and that it helps make your work easier and more efficient. 

If you have any questions or concerns, please don't hesitate to reach out.

Thank you. :]

** : https://github.com/ipluxteamx/Aqua

------------------------------------------------------------------------------------";

        public static void WriteReadme()
        {
            System.IO.File.WriteAllText(@"0:\ReadMe.atf", readMeString);
        }
    }
}
