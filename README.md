# WarframeInventoryAuditor
A tool to audit user inventory in warframe.

Currently it contains the main inventory scanner, relics and mod analysis forms, and a form to show and compare item rewards in 4 relics (useful during relic runs).
I am not very experienced with any of the tools used, so code is likely subpar and not free of bugs.
The warframe market api specifies a maximum of 3rps, which is why it may take a while for things to load the first time around.
Note that it is extreemly easy to manipulate warframe market statisitics. Do not trust the values here 100%. If something seems wrong, it probably is.

The inventory scanner will likely not work if your screen is not the same size as mine. (1920x1080) Be sure to have the game in Borderless Fullscreen mode before atempting to scan.
To scan open up the inventory menu and open the "Prime Part" tab.
Be sure the scroll bar at the bottom is all the way to the left and then hit the audit button on this application. It is best if it is open in another window, but it should work either way.
Wait until the application automatically scrolls through the entire tab to do anything.
It should then load and display information about the value of each of your prime parts. Be aware that it is not correct 100% of the time and there will be a few errors.

The other parts of the application can be accessed by hitting thier respective buttons on the top of the application. They should be relativly easy to use and understand.
The bottom parts of the main form are used when debugging, they can be ignored.
