#!/usr/bin/env python
# -*- coding: utf-8 -*-


def solve_1(data):
    return 0


def solve_2(data):
    return 0


def main():
    from AOC2017 import ensure_data

    ensure_data(24)
    with open('input_24.txt', 'r') as f:
        data = f.read()

    print("Part 1: {0}".format(solve_1(data)))
    print("Part 2: {0}".format(solve_2(data)))


if __name__ == '__main__':
    main()
