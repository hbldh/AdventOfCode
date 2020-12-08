def solve(data):
    rules = {}
    for row in data.splitlines():
        identifier, contents = row.split(' bags contain ')
        if contents.strip() != "no other bags.":
            _c = tuple((int(c[0]), c[1]) for c in (c.strip().strip('.').strip('bags').strip('bag').strip().split(' ', 1) for c in contents.split(',')))
            rules[identifier] = _c
        else:
            rules[identifier] = ()

    return solve_part_1(rules, 'shiny gold'), solve_part_2(rules, 'shiny gold')


def contains(rule, rules, sought):
    if any([c[1].strip() == sought for c in rules[rule]]):
        return True
    for content in rules[rule]:
        if contains(content[1].strip(), rules, sought):
            return True
    return False


def n_bags_within(rule, rules):
    n = 0
    for content in rules[rule]:
        n += content[0] + content[0] * n_bags_within(content[1].strip(), rules)
    return n


def solve_part_1(rules, sought):
    n = 0
    for rule in rules.keys():
        if rule != sought:
            if contains(rule, rules, sought):
                n += 1
    return n


def solve_part_2(rules, sought):
    return n_bags_within(sought, rules)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
