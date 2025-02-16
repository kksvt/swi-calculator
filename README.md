# Calculator

## Overview
This Calculator application has been written in C# using .NET 6. It reads input data from the *input.json* file, with each JSON object being treated as a separate mathematical operation. Currently operations include:
- add
- sub
- mul
- sqrt

## Input syntax
The syntax of each object should be as it follows:
```json
"name": {
    "operator": "",
    "value1": ,
    "value2": 
},
```

## Field explanations
| Field | Description |
| - | - |
| name | an identifier, ought to be unique within the file |
| operator | one of the valid operators |
| value1 | the first number |
| value2 | the second number, should the operator require two operands |

## Output
The result of each operation is saved into an *output.txt* file, with the result preceeded by the *name* (*identifier*). The results are sorted in ascending order.
