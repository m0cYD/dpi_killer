How does it work?
-----------------

In short, it exploits the TCP segmentation mechanism and splits one of the outbound packets into 2 chunks. Since many DPI firewalls do not reconstruct packets before inspection, they miss the packet which contains the blocked website address.

DPI (Deep Packet Inspection) methods are signature-aware and inspect specific offsets of packets to find the destination website address. By shrinking the packet into two parts, what they see is two independent malformed packets and let them pass.

Shrinking and reassembling TCP segments are done solely on the source and the ultimate destination machines and it is extremely difficult for the nodes in the path to keep track of every TCP session and reassemble all shrunk TCP segments.

Since on highly restricted networks, DNS blocking is also an issue, this app also runs a DNS encryption tool and redirects all outgoing DNS requests through it.

This app is created to be used in Iran and only works for HTTPS websites.

--------------------------------------------------

Please note
------------

1- This tool does not change your IP.

2- You can't bypass DPI for HTTP websites by this app.

3- Before shutting down, make sure you have closed the app.

4- If your machine was shut down while the app was running, run and close it again to revert the system network configuration to the previous state.


--------------------------------------------------

Other info
----------

The default .NET version is 4.5, so compiled executable will work on Windows 8+.

It contains dnscrypt_proxy.exe from https://github.com/jedisct1/dnscrypt-proxy and goodbyeapi.exe from https://github.com/ValdikSS/GoodbyeDPI.


