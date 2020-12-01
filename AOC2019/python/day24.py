#!/usr/bin/env python3

import os
import pathlib
import sys

class Planet():
  _TOP_EDGE = set(range(5))
  _BOTTOM_EDGE = set(range(20, 25, 1))
  _LEFT_EDGE = {0, 5, 10, 15, 20}
  _RIGHT_EDGE = {4, 9, 14, 19, 24}

  def __init__(self, up=None, down=None):
    self.grid = [0] * 25
    self.bugs = 0
    self.up = up
    self.down = down

    self.top_edge = 0
    self.bottom_edge = 0
    self.left_edge = 0
    self.right_edge = 0

  def __str__(self):
    output = ""
    for y in range(5):
      for x in range(5):
        output += "#" if self.grid[y * 5 + x] else "."
      output += "\n"
    return output.strip()

  def state(self):
    return tuple(i for i, v in enumerate(self.grid) if v == 1)

  def biodiversity(self):
    return sum(2**n for n in self.state())

  def calc(self):
    self.bugs = sum(self.grid)
    self.top_edge = sum(self.grid[x] for x in Planet._TOP_EDGE)
    self.bottom_edge = sum(self.grid[x] for x in Planet._BOTTOM_EDGE)
    self.left_edge = sum(self.grid[x] for x in Planet._LEFT_EDGE)
    self.right_edge = sum(self.grid[x] for x in Planet._RIGHT_EDGE)

  def parse(self, data):
    for y, row in enumerate(data.split("\n")):
      for x, cell in enumerate(row.strip()):
        self.grid[y * 5 + x] = 1 if cell == "#" else 0
    self.calc()

  def evolve(self, recurse=False):
    if recurse == False:
      return self._simple_evolve()

    if self.up != None:
      return self.up.evolve(recurse)

    # check the edges to see if we need another level up
    if self.top_edge in {1, 2} or \
       self.bottom_edge in {1, 2} or \
       self.left_edge in {1, 2} or \
       self.right_edge in {1, 2}:
      self.up = Planet(None, self)
      return self.up.evolve(recurse)

    self._deep_evolve([])

  def total_bugs(self):
    if self.up is not None:
      return self.up.total_bugs()

    bugs = 0
    planet = self

    while planet is not None:
      bugs += planet.bugs
      planet = planet.down

    return bugs

  def _deep_evolve(self, updates):

    # check the center to see if we need another level down
    if self.down is None and sum(self.grid[x] for x in (7, 11, 13, 17)) > 0:
      self.down = Planet(self)

    for i, val in enumerate(self.grid):
      if i == 12:
        continue

      indexes = []
      if i - 5 >= 0:
        indexes.append(i - 5)
      if i + 5 < 25:
        indexes.append(i + 5)
      if i // 5 == (i + 1) // 5:
        indexes.append(i + 1)
      if i // 5 == (i - 1) // 5:
        indexes.append(i - 1)

      adj = sum(self.grid[x] for x in indexes if x != 12)

      if self.up is not None:
        if i in Planet._TOP_EDGE:
          adj += self.up.grid[7]
        if i in Planet._BOTTOM_EDGE:
          adj += self.up.grid[17]
        if i in Planet._LEFT_EDGE:
          adj += self.up.grid[11]
        if i in Planet._RIGHT_EDGE:
          adj += self.up.grid[13]

      if self.down is not None:
        if i == 7:
          adj += self.down.top_edge
        elif i == 17:
          adj += self.down.bottom_edge
        elif i == 11:
          adj += self.down.left_edge
        elif i == 13:
          adj += self.down.right_edge

      if val == 1 and adj != 1:
        updates.append((self, i, 0))
      elif val == 0 and adj in {1, 2}:
        updates.append((self, i, 1))

    if self.down is None:
      last_planet = self
      for planet, i, val in reversed(updates):
        if planet != last_planet:
          last_planet.calc()
          last_planet = planet

        planet.grid[i] = val

      last_planet.calc()

    else:
      self.down._deep_evolve(updates)

  def _simple_evolve(self):
    updates = []
    for i, val in enumerate(self.grid):
      indexes = []
      if i - 5 >= 0:
        indexes.append(i - 5)
      if i + 5 < 25:
        indexes.append(i + 5)
      if i // 5 == (i + 1) // 5:
        indexes.append(i + 1)
      if i // 5 == (i - 1) // 5:
        indexes.append(i - 1)

      adj = sum(self.grid[x] for x in indexes)

      if val == 1 and adj != 1:
        updates.append((i, 0))
      elif val == 0 and adj in {1, 2}:
        updates.append((i, 1))

    for i, val in updates:
      self.grid[i] = val


def run(data):
  lines = data

  planet = Planet()
  planet.parse(lines)

  seen = set()

  while True:
    state = planet.state()
    if state in seen:
      break

    seen.add(state)
    planet.evolve()

  biodiversity = planet.biodiversity()
  print(f"First repeat biodiversity: {biodiversity}")

  planet = Planet()
  planet.parse(lines)

  for i in range(200):
    planet.evolve(True)

  print(f"Bugs: {planet.total_bugs()}")
  return biodiversity, planet.total_bugs()


def solve(data):

    return run(data)


if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)