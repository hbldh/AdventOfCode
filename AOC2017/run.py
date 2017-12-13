#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os

from AOC2017 import ensure_data


def main(which_days):
    for day in which_days:
        ensure_data(day)
        print("Solutions to Day {0:02d}\n-------------------".format(day))
        # Horrible way to run scripts, but wtf.
        day_module = __import__('{0:02d}'.format(day))
        print('')


if __name__ == "__main__":
    import argparse

    parser = argparse.ArgumentParser("Advent of Code AOC2017 - hbldh")
    parser.add_argument('day', nargs='*', default=None, help="Run only specific day's problem")
    parser.add_argument('--token', type=str, default=None, help="AoC session token. Needed to download data automatically.")
    args = parser.parse_args()

    if args.token:
        os.environ["AOC_SESSION_TOKEN"] = args.token

    if args.day:
        days = map(int, args.day)
    else:
        days = range(1, 3)

    main(days)
