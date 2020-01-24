# FontLink Settings

It looks like this project is getting a bit attention. This revision should clear things up. Please do pull and read this update before messing with your computer using this program. It may or may not be something you want.

## FontLink, Explained

**FontLink** is a series of Windows [Registry](https://docs.microsoft.com/en-us/windows/win32/sysinfo/registry) keys controlling Font Linking, a technology that combines multiple typefaces (fonts) behind-the-scene to substitue missing glyphs (characters) in the currently selected/used typeface.

You can read the full documentation in [the Font Linking section of Font Technology](https://docs.microsoft.com/en-us/globalization/input/font-technology).

## Notes on Terms

*If you're reading a translation (non-English version) of this document, please skip this section as whatever I say here may not apply to other languages.*

To be clear, for most people who are not in the design and print industry, **the word *font* does not originally mean what we think it means**. In typography, *font* is a combination of a typeface (e.g. Times New Roman), a size (e.g. 12 pt) and other attributes (such as weight). So "Times New Roman, 12 pt, bold" is a *font*; while "Times New Roman" is not a *font*, it's only a *typeface*. Similarly, "Segoe UI" is not a *font*, neither is "Arial" or "Microsoft JhengHei".

But in this article, (and many, many others on the Internet,) ***font* is now a catch-all phrase** used colloquially to mean: (1) a typeface (e.g. "Times New Roman"), or (2) a typeface with certain size and attributes (e.g. "Times New Roman, 12 pt, bold"), or rarely (3) just the size/attributes of an unspecified typeface (e.g. "any 12 pt font", "a bolder font"). Much like the widely misused term [*RJ45*](https://en.wikipedia.org/wiki/Registered_jack#Similar_jacks_and_unofficial_names) (which is just a generic 8P8C connector).

Just so you know.

## See It in Action

When you set your Windows display language to English (or many other languages that use Latin script), the default user interface font is Segoe UI ("see-go, U-I"). Segoe font family covers most glyphs used in languages that have European root.

When Windows encounters a glyph that cannot be found in Segoe family, for example the CJK-character "中", in order to render this glyph Windows searches typefaces defined in the *FontLink* Registry for Segoe UI (that is, *HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\FontLink\SystemLink\Segoe UI*) for a substitute glyph. The first entry is Tahoma, not a font designed for CJK scripts; so continue on, the character "中" would be found in either Meiryo (for Japanese) or SimSun (for Chinese), depends on font availability.

Problem is, [Microsoft did many redesigns of typefaces throughout releases of Windows](https://docs.microsoft.com/en-us/globalization/input/font-support). Neither Meiryo or SimSun is the recommended user interface font for CJK characters anymore. In Windows 10, the default font for CJK scripts are:

- **Microsoft YaHei UI** for Chinese (Simplified)
- **Microsoft JhengHei UI** for Chinese (Traditional)
- **Yu Gothic UI** for Japanese
- **Malgun Gothic** for Korean

That's why CJK characters in the following screenshot look so out of place when mixed with Latin characters:

<img alt="File Explorer Screenshot with default Font Linking" src="https://user-images.githubusercontent.com/3415065/52777220-3d104600-307e-11e9-81b8-0d3d745291ff.png">

Using this program, prioritizing any of the above font over Meiryo and SimSun as a substitute for Segoe UI produces the following screenshot (after a logout):

<img alt="File Explorer Screenshot with tuned Font Linking" src="https://user-images.githubusercontent.com/3415065/52777235-43062700-307e-11e9-8a37-9051bbd04026.png">

You can clearly see that the font for CJK characters is changed to a much more look-alike version, while the English text remains from Segoe UI.

## Pros & Cons

Font Linking works great only when you're dealing with multiple languages that use different scripts with no intersecting Unicode codepoints. It doesn't solve all multilingual problems. And frankly, with the rise of Unicode, those problems remain unsolvable.

"Unihan" is one of those problems. When the same glyph/codepoint is used in different scripts and all of these scripts are mixed into one sentence. Windows lacks the necessary information to pick *the best* fallback for this glyph. For example, the character "中" is used in both modern day Simplified and Traditional Chinese, and Japanese. All three scripts have different typefaces to render "中" from, but in Font Linking, you can only pick one typeface (YaHei, JhengHei, or Yu Gothic) to render all "中"s. That means if you prioritized Microsoft YaHei in FontLink Registry, "中" will always to rendered using YaHei even when you're seeing Japanese file names, which should be rendered using Yu Gothic.

**Font Linking also works across Windows. There is no fine-tune per app switch.** Whenever an application (e.g. File Explorer, Notepad) requests a character from a font that has no such character, Windows uses FontLink Registry to do its magic. However, many apps supply typefaces internally or define their own fallback mechanism; these apps will not be affected by Font Linking initially.

## How to Use

This example shows how to use this program to achieve the result in the screenshots.

1. You need to identify the font currently being used/selected in an app. That is, the font that's missing glyphs. In this example, it's the default user interface font for English editions of Windows: Segoe UI.

2. Launch the program (preferably with Administrator privilege). Find and select Segoe UI from top dropdown.

   <img alt="2020-01-24_17-33-12" src="https://user-images.githubusercontent.com/3415065/73058970-aa079f00-3ecf-11ea-891d-0d59ef185c21.png">

3. The list below the dropdown shows the current Registry reading of FontLink fallback orders. Adjust the list by dragging and dropping entries around. **Note that entries with the same name (usually one with and one without GDI attributes) must go together when reordering. Otherwise legacy apps using older technology may not observe new changes.**

   These are the recommended preferences for mixing English and Simplified Chinese characters:

   <img alt="FontLink Settings screenshot for mixing English with Simplified Chinese" src="https://user-images.githubusercontent.com/3415065/73059365-8bee6e80-3ed0-11ea-81af-787dcaf2190e.png">

4. Finally, click *Save Current Settings to Registry*. You may be asked to review the raw Registry value, or to relaunch the program with Administrative privilege if not done so before.

5. To see your changes, logout and log back in. **Note that this may close all currently running programs, and you may lose unsaved work.**

------

At step 3, if you frequently mix English with Traditional Chinese characters, you may want to use these preferences:

<img alt="FontLink Settings screenshot for mixing English with Traditional Chinese" src="https://user-images.githubusercontent.com/3415065/73060042-f48a1b00-3ed1-11ea-8433-6962fd5ffc6f.png">

And this is for mixing English with Japanese characters:

<img alt="FontLink Settings screenshot for mixing English with Japanese" src="https://user-images.githubusercontent.com/3415065/73060134-27ccaa00-3ed2-11ea-9b65-681ddfad26a9.png">
