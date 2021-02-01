from turtle import *

penup()
color('green', 'yellow')
speed(0)
setpos(-400, -200)
pendown()

grammar = "X"

F_Rule = "FF"
X_Rule = "F+[[X]-X]-F[-FX]+X"

n = 6
size = n

forwardStep = 5
angleStep = 25
left(60)
turtAngle = 0

# creating the grammar string
while n > 0:
    print("#### n = " + str(n) + " ####")
    grammarList = list(grammar)
    print(grammarList)

    index = 0
    print("Full grammar: " + grammar)
    for char in grammar:
        if char == "F":
            grammarList[index] = F_Rule
        elif char == "X":
            grammarList[index] = X_Rule

        index = index + 1

    print(grammarList)
    grammar = "".join(grammarList)
    n = n - 1


# parsing the grammar
def realignTurtle(angle):
    global turtAngle
    #print("Setting angle from " + str(turtAngle) + " to " + str(angle))
    while turtAngle != angle:
        if(turtAngle > angle):
            left(angleStep)
            turtAngle = turtAngle - angleStep
        else:
            right(angleStep)
            turtAngle = turtAngle + angleStep

def ParseGrammar(index, level):
    #print("##### Entered recursive level " + str(level) + " #####")
    personalIndex = index
    #print(pos())
    turtlePos = pos()
    global turtAngle
    turtleAngle = turtAngle
    #print(turtlePos)
    while personalIndex < len(grammar):
        char = grammar[personalIndex]
        #print("Currently parsing " + str(char) + " at index " + str(personalIndex) + " at level " + str(level))
        if char == "F":
            forward(forwardStep)
        elif char == "-":
            right(angleStep)
            turtAngle = turtAngle + angleStep
        elif char == "+":
            left(angleStep)
            turtAngle = turtAngle - angleStep
        elif char == "[":
            turtlePos = pos()
            personalIndex = ParseGrammar(personalIndex + 1, level + 1)
            #print("### Backed out into level " + str(level) + "###")
            penup()
            setpos(turtlePos)
            realignTurtle(turtleAngle)
            pendown()
        elif char == "]":
            return personalIndex

        personalIndex = personalIndex + 1
        #input()
        
    return

tracer(0,0)
print("Starting parse")
ParseGrammar(0, 0)
print("Finished")
update()
