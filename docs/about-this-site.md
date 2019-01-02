---
layout: default
---
# About this site

This site was created with [GitHub Pages][ghp]. In the process of learning about GitHub Pages, we decided to also document what we learned.

## Starting off

We started out simply by creating [this repository][this] and editing using the GitHub user interface in a browser. We were able to set up the initial skeleton of the site with a few pages. 

We decided on the approach of using a folder named `docs` located in the master branch. See the [GitHub Pages][ghppub] docs for other choices. After seeing that we could choose a theme in our repository setting, we choose a [theme][ghptheme] and build our docs. We immediately got a decent looking site right-out-of-the-box. Behind the scenes, GitHub Pages takes what you have checked in and automatically generates your web site from any HTML or markdown files you have in the document root.

So far so good, but we wondered about how we could have a table of contents (TOC) show up in the left frame of our rendered site. This led us to reading about [GitHub Pages and Jekyll][ghpjek]. Jekyll is what turns markdown files into HTML. We ended up doing following:

* Copy our Jekyll theme's layout `_layouts/default.html` file into our project and modify it.
* Add custom style in `assets/css/travelmarx.css`.
* Add a `_data/toc.yaml` file to describe our table of contents. The `_data` folder is a Jekyll directory [convention][jek2] conventionand after reading this [YAML tutorial][yamltut], it all started to come together on how to create a table of contents.

Our table of contents file, `toc.yaml`, looks like this:

```yaml
toc:
  - title: Introduction
    url: index
  - title: Get Started
    url: get-started
    sections:
      - title: Run local
        heading: run-local
      - title: Run live
        heading: run-live
  ...
```

The code to read the `toc.yaml` appears in the `default.html` file and is logically this:

```javascript
for itemLevel1 in site.data.toc.toc
    link = itemLevel1.url, text = itemLevel1.title
        for itemLevel2 in itemLevel1.sections
            link = itemLevel1.url#itemLevel2.heading, text = itemLevel2.title
            for itemLevel3 in itemLevel2.sections
                link = itemLevel1.url#itemLevel3.heading, text = itemLevel3.title
            endfor
        endfor
endfor
```

After almost one hundred small commits with the GitHub user interface via a browser, it became obvious that the next step was to build the docs locally because the browser, even with multiple windows, was not a good solution for bulk writing and meaningful commits.

## Running locally

Running our site locally, enables

* Editing offline and with different tools and environments. (We use [GitHub Desktop][desktop] and [Visual Studio Code][vscode].)
* Checking in many changes at once.
* Speeding up write-build cycle by finding and fixing build problems quicker.

For more information about running Jekyll locally, see [Setting up your GitHub Pages site locally with Jekyll][ghpjekloc]. After following the help, our site wasn't rendering correctly and we realized that while working only on-line (with the GitHub UI) our pages didn't need what's called *front matter*, but locally it was needed for each page. A simple *front matter* looks like this:

```md
---
layout: default
---
```
The next thing we realized was that we would need a `Gemfile` and a `.gitignore` to avoid checking in local files we didn't need checked, including the locally built site HTML files. 

Here's an overview of the key commands used in [Git Bash][gitbash]:

```bsh
$ git init travelmarx
$ cd travelmarx
$ get clone https://github.com/travelmarx/scrapbook101.git
```
Create Gemfile base as suggested by the [help][ghpjekloc] page.

```bsh
$ bundle install
$ cd docs
$ bundle exec jekyll serve
```

Notes:

* You have to restart (`bundle exec jekyll serve`) to pick up changes in the `_config.yml`. For example, if you add a new site variable.

* If something doesn't seem right with the build, check the build window started in your Git-Bash window. This is where your big time savings comes in because you can see what the problem is right away and address it.

## Customization

After running for a few weeks, we wanted to add [tabbed content][tabs] using Bootstrap. This lead us discover that there is a GitHub Page theme with [Bootstrap 4 startup site][bootstraptheme]. Instead of using this, we ended up injecting the necessary scripts into the `_layouts\default.html`.

[ghp]: https://pages.github.com/
[ghppub]: https://help.github.com/articles/configuring-a-publishing-source-for-github-pages/
[ghptheme]: https://help.github.com/articles/adding-a-jekyll-theme-to-your-github-pages-site/
[ghpjek]: https://help.github.com/articles/about-github-pages-and-jekyll/
[ghpjekloc]: https://help.github.com/articles/setting-up-your-github-pages-site-locally-with-jekyll/
[this]: https://github.com/travelmarx/scrapbook101
[jek]: https://jekyllrb.com/
[jek2]: https://jekyllrb.com/docs/structure/
[yamltut]: https://idratherbewriting.com/documentation-theme-jekyll/mydoc_yaml_tutorial.html
[gitbash]: https://gitforwindows.org/
[desktop]: https://desktop.github.com/
[vscode]: https://code.visualstudio.com/
[tabs]: https://getbootstrap.com/docs/4.0/components/navs/#tabs
[bootstraptheme]: https://nicolas-van.github.io/bootstrap-4-github-pages/
