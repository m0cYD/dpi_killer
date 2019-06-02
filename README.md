▪️ How does it work?

In short, it exploits tcp segmentation mechanism and splits one of the outbound packets into 2 chunks. Since available DPI firewalls do not reconstruct the packets before inspection, they miss the packet which contains the blocked website address.

DPI (Deep Packet Inspection) methods are signature-aware and inspect specific offsets of packets to find the destination website address. By shrinking the pakcet into two parts, what they see is two independent malformed packets and let them pass.

Shrinking and reassembling tcp segments are done solely on the source and the ultimate destination machines and it is extremely difficult for the nodes in the path to keep track of every tcp session and reassemble all shrinked tcp segments.

Since on highly restricted networks, DNS blocking is also an issue, this app also runs a DNS encryption tool and redirects all outgoing DNS requests through it.

Default .NET version: 4.5 , so compiled exe will work on Windows 8+.

Contains dnscrypt_proxy.exe from https://github.com/jedisct1/dnscrypt-proxy and goodbyeapi.exe from https://github.com/ValdikSS/GoodbyeDPI.
