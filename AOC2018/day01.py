import os


def solve(data):
    values = list(map(int, data.splitlines()))

    f_reached = set()
    freq = 0
    while True:
        for n in values:
            freq += n
            if freq in f_reached:
                return sum(values), freq
            else:
                f_reached.add(freq)


def main():
    from AOC2018 import ensure_data

    day = __file__.split(os.sep)[-1].strip('day').strip('.py')

    ensure_data(int(day))

    with open(f'input_{day}.txt', 'r') as f:
        data = f.read().strip()

    part_1, part_2 = solve(data)

    print("Part 1: {0}".format(part_1))
    print("Part 2: {0}".format(part_2))


if __name__ == '__main__':
    main()
