#!/usr/bin/env python
# -*- coding: utf-8 -*-
import re
from collections import defaultdict

text_pattern = """In state ([\w]):
  If the current value is 0:
    - Write the value ([0-1]).
    - Move one slot to the (left|right).
    - Continue with state ([\w]).
  If the current value is 1:
    - Write the value ([0-1]).
    - Move one slot to the (left|right).
    - Continue with state ([\w])."""


def solve_1(data):
    state, n_steps = re.search(
        "Begin in state ([\w]).\nPerform a diagnostic "
        "checksum after ([\d]+) steps.", data, re.M).groups()
    rules = {}
    for match in re.findall(text_pattern, data, re.M):
        rules[match[0]] = ((int(match[1]),
                            1 if match[2] == 'right' else -1,
                            match[3]),
                           (int(match[4]),
                            1 if match[5] == 'right' else -1,
                            match[6]))
    tape = defaultdict(lambda: 0)
    cursor = 0
    for k in range(int(n_steps)):
        value, direction, state = rules[state][tape[cursor]]
        tape[cursor] = value
        cursor += direction
    return sum(tape.values())


def main():
    from _aocutils import ensure_data

    ensure_data(25)
    with open('input_25.txt', 'r') as f:
        data = f.read()

    print("Part 1: {0}".format(solve_1(data)))


if __name__ == '__main__':
    main()
