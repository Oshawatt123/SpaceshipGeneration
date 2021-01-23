from turtle import *

penup()
color('green', 'yellow')
speed(0)
setpos(-200, 0)
pendown()

grammar = "F"

F_Rule = "F+F-F-F+F"

n = 5
size = n
print(str(size**2))

while n > 0:
    print("#### n = " + str(n) + " ####")
    grammarList = list(grammar)

    index = 0
    print("Full grammar: " + grammar)
    for char in grammar:
        if char == "F":
            grammarList[index] = F_Rule
            
        index = index + 1

    print(grammarList)
    grammar = "".join(grammarList)
    n = n - 1



for char in grammar:
    if char == "F":
        forward(50/(size**2))
    if char == "+":
        left(90)
    if char == "-":
        right(90)

penup()
goto(0,0)
