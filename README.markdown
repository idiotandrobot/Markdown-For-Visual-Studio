Markdown Helper For Visual Studio
=================================

About
------

Use the amazing text-to-HTML tool [Markdown](http://daringfireball.net/projects/markdown) to create HTML in Visual Studio.

You can create a markdown file directly in Visual Studio and this tool will output the generated HTML as a file within the project.  

*I am currently using it to output a partial view for ASP.NET MVC3 for a help/FAQ page.*

*It allows me to write the page using Markdown and have the generated file used as a partial view without me having to copy and paste from a markdown converter to Visual Studio.*

How To
------

###Enabling the tool

Set the "Custom Tool" property on each of your markdown files to "Markdown".

The HTML output will now be automatically created every time you save the markdown source file.

###Naming Convention for your markdown files

The tool can infer the file extension to output using an inline value:

	filename.[required_extension].md or filename.[required_extension].markdown

####Examples

  `test1.html.md` -> `test1.html`  
  `test1.cshtml.md` -> `test1.cshtml`  
  `test1.html.markdown` -> `test1.html`  
   
or alternatively you can use the Custom Tool Namespace property on the Markdown file

####Examples

**Custom Tool Namespace**: html

  `test1.md` -> `test1.html`  
  `test1.markdown` -> `test1.html`  
  `test1.txt` -> `test1.html`  

####N.B.

1. An inline value will override a namespace value.
2. If a namespace value is not specified the Markdown file Project "Default namespace" property will be used.

###Debugging the tool

Requires [Visual Studio 2010 SDK](http://www.microsoft.com/en-gb/download/details.aspx?id=2680) (there is an [SP1](http://visualstudiogallery.msdn.microsoft.com/25622469-19d8-4959-8e5c-4025d1c9183d/) but it refused to install on my system).

In the Project Debug tab set Start Action to "Start external program" and enter the path to the Visual Studio 2010 devenv.exe. 
Set "Command line arguments" to '/rootsuffix Exp'.
	
Credits
=======
- A vast majority of the code in this plugin is taken from Steve Potter's excellent [**LessCssForVisualStudio extension**](https://github.com/StevePotter/LessCssForVisualStudio)
- The markdown conversion is done using [**markdownsharp**](http://code.google.com/p/markdownsharp/)
- Obviously the whole reason for doing this is to use Markdown so all credit to [Markdown](http://daringfireball.net/projects/markdown/)