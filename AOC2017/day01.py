def solve_1(data):
    return sum([int(i) for i, j in zip(data, data[1:] + data[0]) if i == j])

def solve_2(data):
    return sum([int(i) for i, j in zip(data, data[len(data)//2:] +
                                       data[:len(data)//2]) if i == j])


def main():
    from AOC2017 import ensure_data

    ensure_data(1)
    with open('input_01.txt', 'r') as f:
        data = f.read().strip()

    print("Part 1: {0}".format(solve_1(data)))
    print("Part 2: {0}".format(solve_2(data)))


if __name__ == '__main__':
    main()
