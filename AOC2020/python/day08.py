import re
from AOC2020.python.hgc import HandheldGameConsole


def solve(data):
    hgc = HandheldGameConsole(data, lambda x: x.i in x.history)
    hgc.run()
    part_1 = hgc.accumulator

    for jmp in re.finditer("jmp", data, re.MULTILINE):
        this_data = data[: jmp.start()] + "nop" + data[jmp.end() :]
        hgc = HandheldGameConsole(this_data, lambda x: x.i in x.history)
        hgc.run()
        if hgc.terminated_cleanly:
            return part_1, hgc.accumulator
    for nop in re.finditer("nop", data, re.MULTILINE):
        this_data = data[: nop.start()] + "jmp" + data[nop.end()]
        hgc = HandheldGameConsole(this_data, lambda x: x.i in x.history)
        hgc.run()
        if hgc.terminated_cleanly:
            return part_1, hgc.accumulator


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
