#!/usr/bin/env node

// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

const fs = require("fs");
const glob = require("glob");
const program = require('commander');
const yaml = require("yaml").default;


program
  .version(require("../package.json").version)
  .option("-i, --input [path]", "Path to 'Assets' directory of Unity project.", "./Assets/")
  .option("-o, --output [path]", "Path to POT output file.", "./en-US.pot")
  .parse(process.argv);


glob(program.input + "/**/*.{prefab, asset, unity}", function (error, files) {

  // Step 1 - Extract and collate context and string from language components in asset files.

  let potMap = new Map();

  function registerString(context, value, file) {
    context = context || "";

    let contextEntry = potMap.get(context);
    if (!contextEntry) {
      contextEntry = new Map();
      potMap.set(context, contextEntry);
    }

    let stringEntry = contextEntry.get(value);
    if (!stringEntry) {
      stringEntry = {
        references: new Set()
      };
      contextEntry.set(value, stringEntry);
    }

    stringEntry.references.add(file);
  }

  for (let file of files) {
    let fileContent = fs.readFileSync(file, { encoding: "utf8" });
    let pattern = /xgettext:[^]+?value:[^]+?\n/g;
    let match;

    while (true) {
      match = pattern.exec(fileContent);
      if (!match) {
        break;
      }
      let localizedString = yaml.parse(match[0]).xgettext;
      registerString(localizedString.context, localizedString.value, file);
    }
  }


  // Step 2 - Format strings into a POT file.

  let potOutput = "";

  for (let [contextName, contextEntry] of potMap.entries()) {
    for (let [string, stringEntry] of contextEntry.entries()) {
      for (let reference of stringEntry.references) {
        potOutput += `\n#: ${reference}\n`;
      }

      if (!!contextName) {
        potOutput += `msgctxt "${contextName}"\n`;
      }

      potOutput += `msgid "${string}"\n`;
      potOutput += `msgstr ""\n`;
    }
  }


  // Step 3 - Write formatted POT file.

  fs.writeFileSync(program.output, potOutput, { encoding: "utf8" });
});
