import itertools

_data = """mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)"""

def solve(data: str):
    #data = _data

    allergen_possibilities = {}
    ingredient_rows = []
    for row in data.splitlines():
        first, second = row.split(' (contains')
        ingredients = first.split(' ')
        ingredient_rows.append(ingredients)
        allergens = tuple(map(lambda x: x.strip(), second.rstrip(')').split(',')))
        for allergen in allergens:
            if allergen in allergen_possibilities:
                allergen_possibilities[allergen].intersection_update(ingredients)
            else:
                allergen_possibilities[allergen] = set(ingredients)

    part_1 = 0
    all_allergen_possibilities = set(itertools.chain(*allergen_possibilities.values()))
    for ingredients in ingredient_rows:
        part_1 += len(set(ingredients).difference(all_allergen_possibilities))

    changed = True
    while changed:
        changed = False
        cannot_be = set([list(possibilities)[0] for allergen, possibilities in allergen_possibilities.items() if len(possibilities) == 1])
        for allergen, possibilities in allergen_possibilities.items():
            if len(possibilities) != 1:
                allergen_possibilities[allergen] = possibilities.difference(cannot_be)
                changed = True

    canonical_dangerous_ingredient_list = ",".join([allergen_possibilities[ingredient].pop() for ingredient in sorted(allergen_possibilities.keys())])

    return part_1, canonical_dangerous_ingredient_list



if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
