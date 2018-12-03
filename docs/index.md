## Contents

- [Introduction](#introduction)
  * [Run locally](#run-locally)
  * [Run live](#run-live)
  * [Try this sample](#try-this-sample)
- [Technology Overview](technology-overview)
  * [NoSQL](technology-overview#nosql)
  * [MVC](technology-overview#mvc)
 - [Prototype document](prototype-document)

# Introduction

These docs are the how-to part of the blog post 
[A Personal Information Management System: Introducing Scrapbook][1].
In that post we talk about the ideas behind building our own personal information management system (PIM), that we call Scrapbook. These
document pages and code provide a concrete template for building your own Scrapbook application. It's sort of a Scrapbook 101 app to get
you up and going.

If you don't feel like running this code and just want to see it in action, a running sample version of this code with sample data is provided here [TBD][2].

These docs are a work in progress. :runner:

## Run locally

We build this using using Microsoft tools and software. You can get started with not cost to you with the following software running locally:

* [Visual Studio Community][3]
* [Azure Cosmos DB Local Emulator][4]

## Run live

When you have played around locally and you want to take the next step by publishing your site live, you would add the following pieces:

* Azure Cosmos DB Service
* Azure Application Service

The Scrapbook 101 code was build by starting with the [ASP.NET MVC][6] To-Do List app and customizing it. We choose the [ASP.NET MVC][6] but there are many other ways to implement what we have here. For example, see the site [ToDo List][5] shows many MVC implementations using JavaScript web apps.

## Try this sample

Clone this repository
```bash
git clone https://github.com/travelmarx/scrapbook
```

[1]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html
[2]: http://www.travelmarx.com/
[3]: https://visualstudio.microsoft.com/downloads/
[4]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
[5]: http://todomvc.com/
[6]: https://www.asp.net/mvc
