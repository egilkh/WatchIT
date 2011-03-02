# WatchIT

A simple file tracker (created, renamed, deleted, edited) that keeps a list
of all changes to a folder.

It was written to track changes to projects I was involved in. Projects that
had no source control, the source was in a ftp or I was just handed a folder.
When these project has a huge size and changes over time it becomes hard to
know what files has been changed. And thus I decided to write a tool to help
me keep track of things.

## Features

* Add/Remove *paths* to monitor.
* A information window for each *Path* showing what has changed.
* Ability to remove changes (one by one and everything)
* DragDrop a change to another window (FileDrop)

DragDrop functions carry over either the folder or file path. I use it to
upload files via FileZilla, or open them in editor.

## Screenshots

* [MainWindow #1][ss1], shows the main window with full paths in the listing.
* [MainWindow #2][ss2], shows the main window with only the folder name in the list.
* [Path Info #1][ss3], show information about the selected path.
	
## Misc

Icon was created using the image [My eye][myeye] created by
[orangeacid][orangeacid] on [flickr.com][flickr.com].

[orangeacid]: http://www.flickr.com/photos/orangeacid "orangeacid on flickr.com"
[flickr.com]: http://www.flickr.com "flickr.com"
[myeye]: http://www.flickr.com/photos/orangeacid/234358923 "My eye"
[ss1]: http://www.flickr.com/photos/egilkh/5490675965/ "MainWindow #1"
[ss2]: http://www.flickr.com/photos/egilkh/5490675995/ "MainWindow #2"
[ss3]: http://www.flickr.com/photos/egilkh/5490676021/ "Path Info #1"