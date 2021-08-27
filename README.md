﻿# DevtrainBlockchain

A learning exercise worked on with Sam, to implement a very simple "blockchain" wich supports the following:
* Adding a piece of data with the `data,value` command
* "Closing" a block by by searching for a valid salt (required number of zeros at the start of the hash reduced for testing)
  * To close a block use the `close,name` command.
  * When successful, 100 "funds" are awarded to that name.
* Transfer funds between two names with the `transfer,source,destination,amount` command.
