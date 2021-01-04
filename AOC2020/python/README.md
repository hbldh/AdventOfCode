# Advent of Code 2020

Solutions for [Advent of Code 2020](https://adventofcode.com/2020), written in Python.

To run problem solvers, run the `run.py` script in the Python interpreter:

```bash
$ python run.py --help
usage: Advent of Code 2020 - hbldh [-h] [--token TOKEN] [day [day ...]]

positional arguments:
  day            Run only specific day's problem

optional arguments:
  -h, --help     show this help message and exit
  --token TOKEN  AoC session token. Needed to download data automatically.
```

Execute all days by omitting any number arguments or specify a subset of
days by integer values as input. Add the `--token` argument if you want the
solution runner to download input data from the website as well:

```bash
$ python run.py --token 53616c[...]
```

The token can be found as a cookie pertaining to the Advent of Code website 
in your web browser.


## Summary of days

### Day 16

Fun! Rules and validation. Requires simple sudoku solving for matching fields to value sets.

### Day 17

Conway's Game of Life in 3 and 4 dimensions. Brute force solutions.

### Day 18

Operator precedence. Solved by recursive methods, building a state machine. 

### Day 19

Regex training. Recursive regex for part 2. Horribly ugly solution.


### Day 20

Matching 

### Day 21

Another set intersection and difference task.

### Day 22

Recursive number games. Required recursion reduction through some checks.

### Day 23

Horrible. Using lists. Terrible performance. Takes ~12 days to calculate part 2...
Should use a better data structure or find a pattern. Couldn't be bothered...  