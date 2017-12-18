from collections import defaultdict

def is_value(x):
    return x.isdigit() or (x[0] == '-' and x[1:].isdigit())

def _solve(instructions):
    registers = defaultdict(lambda: 0)
    played_freqs = []
    recovered_freqs = []
    n = 0
    while 0 <= n < len(instructions):
        parts = instructions[n].split(' ')
        if parts[0] == 'snd':
            # Play sound.
            played_freqs.append(registers[parts[1]])
        elif parts[0] == 'set':
            registers[parts[1]] = int(parts[2]) if is_value(parts[2]) \
                else registers[parts[2]]
        elif parts[0] == 'add':
            registers[parts[1]] += int(parts[2]) if is_value(parts[2]) \
                else registers[parts[2]]
        elif parts[0] == 'mul':
            registers[parts[1]] *= int(parts[2]) if is_value(parts[2]) \
                else registers[parts[2]]
        elif parts[0] == 'mod':
            registers[parts[1]] %= int(parts[2]) if is_value(parts[2]) \
                else registers[parts[2]]
        elif parts[0] == 'rcv':
            if registers[parts[1]] != 0:
                recovered_freqs.append(registers[parts[1]])
                return played_freqs[-1]
        elif parts[0] == 'jgz':
            condition = int(parts[1]) if is_value(parts[1]) else registers[
                parts[1]]
            if condition > 0:
                n += int(parts[2])
                continue
        else:
            raise ValueError(str(parts))
        n += 1

    return registers


def solve_1(d):
    return _solve(d)


def solve_2(d):
    return _solve(d)


def main():
    from AOC2017 import ensure_data

    ensure_data(18)
    with open('input_18.txt', 'r') as f:
        data = f.read().strip()
    data2 = """set a 1
add a 2
mul a a
mod a 5
snd a
set a 0
rcv a
jgz a -1
set a 1
jgz a -2"""
    print("Part 1: {0}".format(solve_1(data.splitlines())))
    print("Part 2: {0}".format(solve_2(data.splitlines())))


if __name__ == '__main__':
    main()
