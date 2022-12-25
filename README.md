<h1 align="center">Electronic Bingo</h1>
A C# Application that takes the manual roulette of letter and number combination to Electronic Bingo. It also preview your own patterns to play throughout the game.

# Preview
<p align="center"><img src="https://i.ibb.co/sVC3QtF/ezgif-3-474384f6ce.gif" alt="" /><p>
<p align="center">Picking letter and number combination. Also clearing out the fields</p>
<p align="center"><img src="https://i.ibb.co/wJsmjFJ/ezgif-3-c7d19ad4d6.gif" alt="" /><p>
<p align="center">Selection of Patterns</p>

# Usage
Add this after the <b>InitializeComponent</b> to render the first pattern after opening<br>
Change the string to the specific path of pattern pictures named with 1, 2 and so forth.
```cs
pictureBox2.Image = Image.FromFile(@"(path of file)" + counter + ".png");
```
To speed up the randomizing effect on the screen, change the sleep time in `randomize_letter()` and `randomize_number()`
```cs
Thread.Sleep(50);
```
Change the screen size to your preferred size. I set it as `1920` to maximize my own screen<br>
![image](https://user-images.githubusercontent.com/73803767/209467219-b4d62ae1-57a1-474d-93d2-fd2eef176320.png)

# ⚠️ Bugs
| Known                                                                    | Possible Cause                                                                             |
|--------------------------------------------------------------------------|--------------------------------------------------------------------------------------------|
| Occasionally wrong number combination is being displayed when being clicked | The thread is slow when refreshing the list of numbers that depends on the letter selected |
| After pausing the thread, the combination displayed is different to the list of selected combination     | The thread sleep is too fast, the number moves a little bit after pausing and leads to wrong combination |

Second bug can be fixed with a work around which is putting a longer `Thread.Sleep` to let it refresh the list of number that should come with the selected letter. Basically clicking the button too fast will result to uneven results of combinations

# Contributing
This repository thrives on your contributions! ❤️ To get involved, just create a pull request explaining the request and I'll be happy to merge it if necessary


