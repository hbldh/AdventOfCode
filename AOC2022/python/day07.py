from functools import reduce
import operator


def fs_get(data_dict: dict, map_list: list[str]):
    return reduce(operator.getitem, map_list, data_dict)


def fs_set(data_dict, map_list, value):
    fs_get(data_dict, map_list[:-1])[map_list[-1]] = value


def code_to_tree(commands):
    tree = {"/": {}}
    cwd = ["/", ]
    all_directories = []
    for command in commands:
        if command.startswith("$ cd"):
            _dir = command.replace("$ cd", "").strip()
            match _dir:
                case "/":
                    cwd = ["/", ]
                case "..":
                    cwd.pop()
                case _:
                    cwd.append(_dir)
        elif command.startswith("$ ls"):
            pass
        else:
            if command.startswith("dir"):
                fs_set(tree, cwd + [command.split(" ")[1].strip()], {})
                all_directories.append(cwd + [command.split(" ")[1].strip()])
            else:
                size, file_name = command.split(" ")
                fs_set(tree, cwd + [file_name, ], int(size))
    return tree, all_directories


def dir_sizeof(tree):
    size = 0
    for key in tree:
        if isinstance(tree[key], int):
            size += tree[key]
        else:
            size += dir_sizeof(tree[key])
    return size


def solve(data):
    tree, all_dirs = code_to_tree(data.splitlines())
    return solve_part_1(tree, all_dirs), solve_part_2(tree, all_dirs)


def solve_part_1(tree: dict, all_dirs):
    sizes = [dir_sizeof(fs_get(tree, dir_path)) for dir_path in all_dirs]
    return sum(filter(lambda x: x < 100000, sizes))


def solve_part_2(tree: dict, all_dirs):
    sizes = [dir_sizeof(fs_get(tree, dir_path)) for dir_path in all_dirs]
    required_size_of_dir_to_remove = 30000000 - (70000000 - dir_sizeof(tree))
    return next(filter(lambda x: x > required_size_of_dir_to_remove, sorted(sizes)))


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)
