#### **Version 1.2.1:** (future) Linux type permissions
#### **Version 1.2:** Just reload what you see
Only reload if you have deployed that branch, but only direct children
#### **Version 1.1.1:** Fix unforeseen bugs
- Error when writing two routes types c:\\
- Error when writing only the path type \\192.168.1.X due to not choosing a resource from that address
- Error duplicates the root and has an extra thread for every time you hit reload without it finishing loading
#### **Version 1.1:** Threads
A second thread was added when it reads the directories and draws them in the tree