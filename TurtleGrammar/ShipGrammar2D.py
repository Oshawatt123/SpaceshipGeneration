from turtle import*

forwardStep = 20

angleStep = 90
turtAngle = 0

grammar = "FFF[FF-F-FF]-F-FFF-F[-F+W][--F+F-M]"

C_Rule = "C[FF-F-FF][+F-W][++F-F+M]"

def TurnRight(angle):
    global turtAngle
    right(angle)
    turtAngle = turtAngle + angle

def TurnLeft(angle):
    global turtAngle
    left(angle)
    turtAngle = turtAngle + angle

def DrawCockpit(step):
    forward(step*3)
    TurnRight(90)
    forward(step)
    TurnRight(90)
    forward(step*3)
    TurnRight(90)
    forward(step)

def DrawWing(step, mirror):
    if(mirror):
        TurnLeft(90)
        forward(step/2)
        TurnRight(90)
        
    forward(step*4)
    TurnRight(90)
    forward(step/2)
    TurnRight(90)
    forward(step*4)
    TurnRight(90)
    forward(step/2)

n = 1

while n > 0:
    print("#### n = " + str(n) + " ####")
    grammarList = list(grammar)
    print(grammarList)

    index = 0
    print("Full grammar: " + grammar)
    for char in grammar:
        if char == "C":
            grammarList[index] = C_Rule
        #elif char == "X":
            #grammarList[index] = X_Rule

        index = index + 1

    print(grammarList)
    grammar = "".join(grammarList)
    n = n - 1


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
    print("##### Entered recursive level " + str(level) + " #####")
    personalIndex = index
    #print(pos())
    turtlePos = pos()
    global turtAngle
    turtleAngle = turtAngle
    print("Starting level at " + str(turtlePos))
    while personalIndex < len(grammar):
        char = grammar[personalIndex]
        print("Currently parsing " + str(char) + " at index " + str(personalIndex) + " at level " + str(level))
        if char == "F":
            forward(forwardStep)
        elif char == "-":
            TurnRight(angleStep)
        elif char == "+":
            TurnLeft(angleStep)
            turtAngle = turtAngle - angleStep
        elif char == "[":
            turtlePos = pos()
            personalIndex = ParseGrammar(personalIndex + 1, level + 1)
            print("### Backed out into level " + str(level) +  "at position" + str(turtlePos) +"###")
            penup()
            setpos(turtlePos)
            realignTurtle(turtleAngle)
            pendown()
        elif char == "]":
            return personalIndex
        elif char == "C":
            DrawCockpit(forwardStep)
        elif char == "W":
            DrawWing(forwardStep, False)
        elif char == "M":
            DrawWing(forwardStep, True)

        personalIndex = personalIndex + 1
        #input()
        
    return

print("Parsing grammar" + str(grammar))
ParseGrammar(0, 0)
