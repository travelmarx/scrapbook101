---
layout: default
---
# Introduction

This document set describes how to build a Personal Information Managment (PIM) system as discussed in the related of the blog post [A Personal Information Management System: Introducing Scrapbook][blog]. In that post we describe why we built our own PIM and give a  high-level look at how it was built. We call our PIM system "Scrapbook". This web site discusses in detail how you can create a version of Scrapbook yourself on Windows using ASP.NET. In the interest of reduced complexity, we present here a simplified version we call {{site.sn}}.

To see an example of this code in action, go to [TBD][demo]. Or here's a video of how it works [TBD][demo]. Note that these document pages are still a work in progress as of January 2019. 

This document set covers both {{site.sn}} implemented on the .NET Framework and .NET core.

## A brief recap

The blog post referenced above discusses our reasons for creating a Scrapbook application. To summarize, in our PIM we wanted to:

* Deal with ever-increasing amounts of personal **archival** data, both physical and digital, in a consistent way.
* Capture **context** about data so that when we looked at it in the future we would understood why we saved it and why it was important.
* **Find** our data quickly and on-demand, for example, using natural language and a chat bot.
* **Own** our data and not have it living in a social network or service that locks the format or controls access of it.

The way we could accomplish this was to create a custom PIM system consisting of software, programming frameworks, and cloud services working together to achieve our four principles described above.

While {{site.sn}} is a pared-down version of what we described in the blog post, it has enough of the basic functionality to be a good starting point for someone interested in creating their own Scrapbook.

## Why this site?

Why is {{site.sn}} documented here and not on [blog.travelmarx.com][tm]? For two reasons. First, we were interested in trying out [GitHub Pages][ghp] for technical documentation. And second, demonstrating code and talking about it in a Blogger post is not easy for writer or reader. Hence, this GitHub Pages site was born.

Creating this site was itself a bit of odyssey into new tools and practices that we thought was interesting to document as well. If you want to know a little bit about how this site was created, see [About This Site][about].

[about]: about-this-site
[web]: https://travelmarx.github.io/scrapbook101/
[blog]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html
[demo]: http://www.travelmarx.com/
[tm]: http://blog.travelmarx.com
[ghp]: https://pages.github.com/


