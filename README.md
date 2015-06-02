SDF Encryptor
=============

## Required

Install SQL Server Compact 4.0 via Microsoft Web Platform Installer SP1

## Usage

After cloning the source and building let's leverage the command-line. 

    // The simplest way to start the SDF Encryptor is like SdfEncryptor.exe [filename] [new-password] [old-password]
	// In the example below we'll take a database that doesn't have a password configured and set a new password.
	// If the last parameter is empty we'll assume it's a blank password.
    $ SdfEncryptor.exe file.sdf StrongPassword
	
	// In the example below we'll change the password from an old password to a new password.
	$ SdfEncryptor.exe file.sdf new-password old-password
    
    // And lastly it's possible to remove a password from an SDF DB like this:
	// Note: the word _empty_ will be treated as a blank string "".
	$ SdfEncryptor.exe file.sdf empty old-password