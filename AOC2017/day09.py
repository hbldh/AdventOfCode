def _remove_excl(data):
    out = []
    skip_one = False
    for c in data:
        if c == '!':
            skip_one = True
            continue
        elif skip_one:
            skip_one = False
            continue
        out.append(c)
    return "".join(out)

def _remove_garbage(data):
    out = []
    in_garbage = False
    for c in data:
        if c == '<':
            in_garbage = True
            continue
        if in_garbage and c == '>':
            in_garbage = False
            continue
        if not in_garbage:
            out.append(c)
    return "".join(out)


def _extract_root_groups(data):
    out = []
    group = []
    group_depth = 0
    in_group = False
    for c in data:
        if c == '{' and not in_group:
            group_depth += 1
            in_group = True
        elif c == '}' and in_group:
            group_depth -= 1
            if group_depth == 0:
                out.append(c)
                group.append("".join(out))
        if in_garbage and c == '>':
            in_garbage = False
            continue
        if not in_garbage:
            out.append(c)
    return "".join(out)



def solve_1(indata):
    data = _remove_excl(indata)
    data = _remove_garbage(data)
    gropus = _extract_root_groups(data)
    return 0


def solve_2(indata):
    return 0


def main():
    from AOC2017 import ensure_data

    ensure_data(9)
    with open('input_09.txt', 'r') as f:
        data = f.read().strip()

    data = """{{<a!>},{<a!>},{<a!>},{<ab>}}"""

    print("Part 1: {0}".format(solve_1(data)))
    print("Part 2: {0}".format(solve_2(data)))


if __name__ == '__main__':
    main()
