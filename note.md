### Material
- https://niiconsulting.com/checkmate/2018/02/malware-development-welcome-dark-side-part-1/
- https://www.varonis.com/blog/malware-coding-lessons-people-part-learning-write-custom-fud-fully-undetected-malware/
- https://0x00sec.org/t/the-art-of-malware-bringing-the-dead-back-to-life/19599/10
- https://0x00sec.org/t/master-of-rats-how-to-create-your-own-tracker/20848
- https://0x00sec.org/t/links-probably-worth-reading/916

#### Here’s a basic concept that is in no way full fleshed-out. The current project name is Murmur.

- Modular - can be expanded at runtime with more modules
- Basic functionality has small binary
- Updatable
- Hides in legitimate dll in process
- Can spread across network
- Uses dns requests for basic communication
- Http for more complex/large communications
- Conceals data in images for communications
- Collects info, passwords, keystrokes, screenshots, etc
- Organized by encrypted, compressed files

#### Communication

- Client
- Uses dns requests to send small amounts of data periodically
- Uses https POST to send larger data
- Uses https GET to get modules or commands
- Encapsulates communications in images
- Server
- Puts received data into encrypted archives for collection
- Has front website to seem legit
- Encrypted Virtual File System
- Stores files in a virtual file system
- Stores modules, data that needs to be sent to C&C, configuration