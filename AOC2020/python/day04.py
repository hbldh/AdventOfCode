import re

reqs = {
    "byr": "(Birth Year)",
    "iyr": "(Issue Year)",
    "eyr": "(Expiration Year)",
    "hgt": "(Height)",
    "hcl": "(Hair Color)",
    "ecl": "(Eye Color)",
    "pid": "(Passport ID)",
    "cid": "(Country ID)",
}
all_fields = set(reqs.keys())


def solve(data):
    passports = []
    this_passport = []
    for row in data.splitlines():
        if not row:
            passports.append([p.split(":", maxsplit=1) for p in this_passport])
            this_passport = []
        else:
            this_passport += row.split(" ")
    passports.append([p.split(":", maxsplit=1) for p in this_passport])

    return solve_part_1(passports), solve_part_2(passports)


def is_valid_1(passport):
    delta = all_fields.difference([field[0] for field in passport])
    if len(delta) == 0:
        return True
    elif len(delta) == 1 and "cid" in delta:
        return True
    else:
        return False


def _is_valid_hgt(param):
    c = re.search("([0-9]*)(cm|in)", param)
    if c:
        value, unit = c.groups()
        if unit == "cm":
            return 150 <= int(value) <= 193
        elif unit == "in":
            return 59 <= int(value) <= 76
        else:
            return False
    return False


def is_valid_2(passport):
    """
    byr (Birth Year) - four digits; at least 1920 and at most 2002.
    iyr (Issue Year) - four digits; at least 2010 and at most 2020.
    eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
    hgt (Height) - a number followed by either cm or in:
        If cm, the number must be at least 150 and at most 193.
        If in, the number must be at least 59 and at most 76.
    hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
    ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
    pid (Passport ID) - a nine-digit number, including leading zeroes.
    cid (Country ID) - ignored, missing or not.
    """

    checks = [
        1920 <= int(passport.get("byr", 0)) <= 2002,
        2010 <= int(passport.get("iyr", 0)) <= 2020,
        2020 <= int(passport.get("eyr", 0)) <= 2030,
        _is_valid_hgt(passport.get("hgt", "0")),
        re.match("#[0-9a-f]{6}", passport.get("hcl", "0")) is not None,
        passport.get("ecl", "0") in ("amb", "blu", "brn", "gry", "grn", "hzl", "oth"),
        re.match("[0-9]{9}", passport.get("pid", "0")) is not None
        and len(passport.get("pid", "0")) == 9,
    ]
    return all(checks)


def solve_part_1(passports):
    return len(list(filter(lambda x: is_valid_1(x), passports)))


def solve_part_2(passports):
    return len(list(filter(lambda x: is_valid_2(dict(x)), passports)))


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)
