from collections import defaultdict

from AOC2018.utils import chunked_iter


def solve(data):
    x, opcode_map = solve_1(data)
    return x, solve_2(data, opcode_map)


def _solve_for_opcode(opcodes):
    while True:
        singles = [list(v)[0] for v in opcodes.values() if len(v) == 1]
        if len(singles) == len(opcodes):
            break
        for s in singles:
            for k in opcodes:
                if s in opcodes[k] and len(opcodes[k]) > 1:
                    opcodes[k].remove(s)
    return {v.pop(): k for k, v in opcodes.items()}


def solve_1(data):
    n_like_three_or_more = 0
    opcodes = defaultdict(lambda: set())

    for before, instr, after in chunked_iter(filter(lambda x: x, data.splitlines()), 3):
        if not before.startswith("Before:"):
            break
        n = 0
        instr = list(map(int, instr.split()))
        regs_after = eval(after.split("After:")[1].strip())
        for opcode in all_opcodes:
            r = eval(before.split("Before:")[1].strip())
            opcode(r, *instr[1:])
            if r == regs_after:
                opcodes[opcode].add(instr[0])
                n += 1
        if n >= 3:
            n_like_three_or_more += 1

    opcode_map = _solve_for_opcode(opcodes)

    return n_like_three_or_more, opcode_map


def solve_2(data, opcode):
    r = [0, 0, 0, 0]
    for instr in data[data.find("\n\n\n") :].strip().splitlines():
        instr = list(map(int, instr.split()))
        opcode[instr[0]](r, *instr[1:])

    return r[0]


def addr(r, a, b, c):
    r[c] = r[a] + r[b]


def addi(r, a, b, c):
    r[c] = r[a] + b


def mulr(r, a, b, c):
    r[c] = r[a] * r[b]


def muli(r, a, b, c):
    r[c] = r[a] * b


def banr(r, a, b, c):
    r[c] = r[a] & r[b]


def bani(r, a, b, c):
    r[c] = r[a] & b


def borr(r, a, b, c):
    r[c] = r[a] | r[b]


def bori(r, a, b, c):
    r[c] = r[a] | b


def setr(r, a, b, c):
    r[c] = r[a]


def seti(r, a, b, c):
    r[c] = a


def gtir(r, a, b, c):
    r[c] = 1 if a > r[b] else 0


def gtri(r, a, b, c):
    r[c] = 1 if r[a] > b else 0


def gtrr(r, a, b, c):
    r[c] = 1 if r[a] > r[b] else 0


def eqir(r, a, b, c):
    r[c] = 1 if r[a] == b else 0


def eqri(r, a, b, c):
    r[c] = 1 if a == r[b] else 0


def eqrr(r, a, b, c):
    r[c] = 1 if r[a] == r[b] else 0


all_opcodes = [
    addr,
    addi,
    mulr,
    muli,
    banr,
    bani,
    borr,
    bori,
    setr,
    seti,
    gtir,
    gtri,
    gtrr,
    eqir,
    eqri,
    eqrr,
]

if __name__ == "__main__":
    from AOC2018 import run_solver

    run_solver(solve, __file__)
