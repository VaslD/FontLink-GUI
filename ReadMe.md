# FontLink Settings

## WTF is FontLink?

FontLink is a series of Windows registry keys that define the fallback order of fonts (typefaces) to use when a glyph cannot be rendered using the current font.

*(Or at least that's what I think it is.)*

It works like that so who cares.

## Your explaination sucks. Where's the official documentation?

[Probably somewhere on microsoft.com](https://docs.microsoft.com/en-us/) but I couldn't find it. DIY!

## How do I know it works like that?

Create a folder on your desktop and name it "汉字". See what the characters look like? Compare them with the screenshots below.

I'm sorry there actually aren't any "汉字" in the screenshots, so just compare the screenshots.

<img width="480" alt="File Explorer Screenshot 1" src="https://user-images.githubusercontent.com/3415065/52777220-3d104600-307e-11e9-81b8-0d3d745291ff.png">

<img width="480" alt="File Explorer Screenshot 2" src="https://user-images.githubusercontent.com/3415065/52777235-43062700-307e-11e9-8a37-9051bbd04026.png">

You can clearly see that the fonts for CJK characters are changed, while the English text remains ~~Times New Roman~~ Segoe UI.

## Cool. Why do I need it again?

You probably don't if your daily routine only deals with languages that use the same character set. If you speak more than one language and frequently works with filenames in more than one language, you may want to fine tune the fallback order of fonts.

FontLink is totally harmless. It doesn't change the font Windows uses globally; nor does it hijack rendering APIs. It's a pretty neat feature that's already working transparently in modern versions of Windows. There's simply no way to change the default setting in a regular user's point of view.

Now with this intuitive tool, you can!

## Define "intuitive".

Drop-down selection with drag-and-drop ordering. Sounds nice?!

## Anything else to know about?

Depends on the security settings, you may need administrative privilege to make changes. The application starts with non-elevated access by default, and reports error if certain access is denied. ~~No self-elevation provided. You must run it as administrator manually.~~

If you're worried, check the source code for virus. If you don't code, ask some friend to check it for you. If you recognize all the words in this section but they don't make sense, you shouldn't use it!

## License?

Free-for-all, unless otherwise specified. It's really just a registry reader/writer, nothing special.
