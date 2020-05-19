<img src="https://github.com/mrklintscher/libfintx/blob/master/res/logo.png" align="right">

# libfintx

[![Build Status](https://travis-ci.org/mrklintscher/libfintx.svg?branch=master)](https://travis-ci.org/mrklintscher/libfintx)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/928e912657d44a6090d329343aa13346)](https://www.codacy.com/app/torsten-klinger/libfintx?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=mrklintscher/libfintx&amp;utm_campaign=badger)
[![Issue Count](https://codeclimate.com/github/mrklintscher/libfintx/badges/issue_count.svg)](https://codeclimate.com/github/mrklintscher/libfintx)

An C# based client library for **HBCI 2.2** and **FinTS 3.0** and **EBICS H004** and **EBICS H005**.

In 1995 the ZKA announced a common online banking standard called *Homebanking Computer Interface* (HBCI). In 2003 they published the next generation of this protocol standard and named it *Financial Transaction Services* (FinTS).

Today most of all german banks support this online banking standards.

The Electronic Banking Internet Communication Standard (EBICS) is a German transmission protocol developed by the German Banking Industry Committee for sending payment information between banks over the Internet. It grew out of the earlier BCS-FTAM protocol that was developed in 1995, with the aim of being able to use internet connections and TCP/IP. It is mandated for use by German banks and has also been adopted by France and Switzerland. [Wikipedia](https://en.wikipedia.org/wiki/Electronic_Banking_Internet_Communication_Standard).

This client library supports all four APIs, HBCI 2.2, FinTS 3.0 and EBICS H004 and H005.

It can be used to read the balance of a bank account, receive an account statement, and make a SEPA payment using PIN/TAN.

# Nuget

| Target | Branch | Version | Download link |
| ------ | ------ | ------ | ------ |
| Nuget | master | v0.0.1 | [![NuGet](https://img.shields.io/badge/nuget-v0.0.1-blue)](https://www.nuget.org/packages/libfintx/) |

# Usage

There are many reasons why you need to use a banking library which can exchange data from your application with the bank. One reason for example is to found a [Fintech](https://de.wikipedia.org/wiki/Finanztechnologie).

# Target platforms

* .NET Standard 2.0

# Sample

Look at the demo projects inside the master branch.

# Features

* Get Balance (**HKSAL**)
* Request Transactions (**HKKAZ**)
* Transfer money (**HKCCS**)
* Transfer money at a certain time (**HKCCS**)
* Collective transfer money (**HKCCM**)
* Collective transfer money terminated (**HKCME**)
* Rebook money from one to another account (**HKCUM**)
* Collect money (**HKDSE**)
* Collective collect money (**HKDME**)
* Load mobile phone prepaid card (**HKPPD**)
* Submit banker's order (**HKCDE**)
* Get banker's orders (**HKCSB**)

# Specification

For exact information please refer to the [german version of the specification](http://www.hbci-zka.de/spec/spezifikation.htm). There is
also an [unauthorized english translation](http://www.hbci-zka.de/english/specification/engl_2_2.htm).

# SSL verification

The verification process is done by using the default [**WebRequest**](https://msdn.microsoft.com/de-de/library/system.net.webrequest(v=vs.110).aspx) class.

# Limitations EBICS

* Usage with certificates has been prepared but not completely implemented yet. It works with private/public keys.
* Only version A005 for signatures can be used. A006 uses PSS padding, which is currently not supported by .NET Core 2.x. Bouncy Castle is only used for PEM file and certificate management.
* Only version E002 for encryption can be used.
* Only version X002 for authentication can be used.
* It was developed using EBICS Version H004, but H005 should work.
* Currently implemented commands/requests: INI, HIA, HPB, PTK, SPR, STA, CCT, CDD

# Copyright & License

Copyright (c) 2016 - 2020 **Torsten Klinger**

Licensed under GNU Lesser General Public License. Please read the LICENSE file.

Parts of this library are based on Bjoern Kuensting NetEbics client. These are licensed under a Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.

See the file LICENSE.txt for further information.

# Support

You can contact me on Telegram. 💬

[![Telegram](https://img.shields.io/badge/telegram-chat-blue)](https://t.me/mrklintscher)
