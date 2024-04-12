using System;
using System.Collections.Generic;
using System.Text;
using Interpreter.Enums;

namespace Interpreter.DataTemplates
{
    internal static class DefaultObjectList
    {
        internal static Dictionary<string, DefaultSignature> Objects { get; private set; }

        internal static void Install()
        {
            if (Objects == null)
            {
                Objects = new Dictionary<string, DefaultSignature>();
            }
            Objects.Clear();
            // ASSERT
            Objects.Add("assert.failed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.equal", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.ANY, VariableType.ANY, VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.notequal", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.ANY, VariableType.ANY, VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.less", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.greater", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.lessequal", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.greaterequal", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("assert.near", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            // BUTTONS
            Objects.Add("buttons.getclicks", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.STRING));
            Objects.Add("buttons.wait", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("buttons.flush", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("buttons.current", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.STRING));
            // BYTE
            Objects.Add("byte.not", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.and_", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.or_", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.xor", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.bit", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.shl", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.shr", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("byte.tohex", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("byte.tobinary", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("byte.tologic", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("byte.h", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("byte.b", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("byte.l", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            // EV3
            Objects.Add("ev3.setledcolor", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.STRING, VariableType.STRING }, VariableType.NON));
            Objects.Add("ev3.systemcall", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("ev3.queuenextcommand", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("ev3.batterylevel", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("ev3.batteryvoltage", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("ev3.batterycurrent", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("ev3.time", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("ev3.brickname", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.STRING));
            // EV3FILE
            Objects.Add("ev3file.openwrite", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("ev3file.openappend", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("ev3file.openread", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("ev3file.close", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("ev3file.writeline", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("ev3file.writebyte", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("ev3file.writenumberarray", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER_ARRAY }, VariableType.NON));
            Objects.Add("ev3file.readline", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("ev3file.readbyte", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("ev3file.readnumberarray", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER_ARRAY));
            Objects.Add("ev3file.converttonumber", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.ANY }, VariableType.NUMBER));
            Objects.Add("ev3file.tablelookup", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            // LCD
            Objects.Add("lcd.stopupdate", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("lcd.update", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("lcd.clear", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("lcd.rect", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.line", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.text", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("lcd.write", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.ANY }, VariableType.NON));
            Objects.Add("lcd.circle", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.fillcircle", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.fillrect", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.inverserect", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.pixel", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("lcd.bmpfile", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            // MAILBOX
            Objects.Add("mailbox.create", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("mailbox.createfornumber", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("mailbox.isavailable", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("mailbox.receive", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("mailbox.send", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.STRING, VariableType.STRING, VariableType.STRING }, VariableType.NON));
            Objects.Add("mailbox.receivenumber", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("mailbox.sendnumber", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.STRING, VariableType.STRING, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("mailbox.connect", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NON));
            // MATH
            Objects.Add("math.pi", new DefaultSignature(ObjectType.PROPERTY, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("math.abs", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.ceiling", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.floor", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.naturallog", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.log", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.cos", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.sin", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.tan", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.arcsin", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.arccos", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.arctan", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.getdegrees", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.getradians", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.squareroot", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.power", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.round", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.getrandomnumber", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.min", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.max", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("math.remainder", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            // MOTOR
            Objects.Add("motor.stop", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.STRING, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.start", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.STRING, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motor.startpower", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.STRING, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motor.startsync", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motor.getspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("motor.isbusy", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.STRING));
            Objects.Add("motor.schedule", new DefaultSignature(ObjectType.METHOD, 6, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.schedulepower", new DefaultSignature(ObjectType.METHOD, 6, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.schedulesync", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.resetcount", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.getcount", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NUMBER));
            Objects.Add("motor.move", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.movepower", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.movesync", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.startsteer", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motor.schedulesteer", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.movesteer", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.STRING, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.invert", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NON));
            Objects.Add("motor.wait", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.STRING }, VariableType.NON));
            // MOTOR_A
            Objects.Add("motora.gettacho", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motora.getspeed", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motora.resetcount", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.setdirectpolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.setreverspolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.islarge", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.ismedium", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motora.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motora.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motora.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motora.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_B
            Objects.Add("motorb.gettacho", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motorb.getspeed", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motorb.resetcount", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.setdirectpolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.setreverspolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.islarge", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.ismedium", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorb.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorb.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorb.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorb.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_C
            Objects.Add("motorc.gettacho", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motorc.getspeed", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motorc.resetcount", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.setdirectpolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.setreverspolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.islarge", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.ismedium", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorc.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorc.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorc.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorc.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_D
            Objects.Add("motord.gettacho", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motord.getspeed", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("motord.resetcount", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.setdirectpolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.setreverspolarity", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.islarge", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.ismedium", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motord.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motord.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motord.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motord.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_AB
            Objects.Add("motorab.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorab.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorab.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorab.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorab.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorab.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorab.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_AC
            Objects.Add("motorac.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorac.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorac.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorac.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorac.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorac.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorac.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_AD
            Objects.Add("motorad.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorad.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorad.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorad.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorad.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorad.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorad.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_BC
            Objects.Add("motorbc.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorbc.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorbc.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorbc.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorbc.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorbc.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorbc.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_BD
            Objects.Add("motorbd.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorbd.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorbd.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorbd.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorbd.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorbd.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorbd.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // MOTOR_CD
            Objects.Add("motorcd.off", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorcd.offandbrake", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorcd.start", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("motorcd.setpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorcd.setspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorcd.startpower", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("motorcd.startspeed", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            // PROGRAM
            Objects.Add("program.delay", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("program.end", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("program.argumentcount", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("program.directory", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.STRING));
            Objects.Add("program.getargument", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            // ROW
            Objects.Add("row.init", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("row.delete", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("row.read", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("row.write", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("row.size", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("row.resize", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            // SENSOR
            Objects.Add("sensor.getname", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("sensor.gettype", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("sensor.getmode", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("sensor.setmode", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("sensor.isbusy", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("sensor.wait", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("sensor.readpercent", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("sensor.readraw", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER_ARRAY));
            Objects.Add("sensor.readrawvalue", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("sensor.communicatei2c", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER_ARRAY }, VariableType.NUMBER_ARRAY));
            Objects.Add("sensor.readi2cregister", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER));
            Objects.Add("sensor.readi2cregisters", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER_ARRAY));
            Objects.Add("sensor.writei2cregister", new DefaultSignature(ObjectType.METHOD, 4, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("sensor.writei2cregisters", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER_ARRAY }, VariableType.NON));
            Objects.Add("sensor.senduartdata", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER_ARRAY }, VariableType.NON));
            // SENSOR1
            Objects.Add("sensor1.raw1", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("sensor1.raw3", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            // SENSOR2
            Objects.Add("sensor2.raw1", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("sensor2.raw3", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            // SENSOR3
            Objects.Add("sensor3.raw1", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("sensor3.raw3", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            // SENSOR4
            Objects.Add("sensor4.raw1", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("sensor4.raw3", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            // SPEAKER
            Objects.Add("speaker.stop", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("speaker.tone", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("speaker.note", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.STRING, VariableType.NUMBER }, VariableType.NON));
            Objects.Add("speaker.play", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.STRING }, VariableType.NON));
            Objects.Add("speaker.isbusy", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.STRING));
            Objects.Add("speaker.wait", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            // TEXT
            Objects.Add("text.append", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.ANY, VariableType.ANY }, VariableType.STRING));
            Objects.Add("text.getlength", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.ANY }, VariableType.NUMBER));
            Objects.Add("text.issubtext", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.ANY, VariableType.ANY }, VariableType.STRING));
            Objects.Add("text.endswith", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.ANY, VariableType.ANY }, VariableType.STRING));
            Objects.Add("text.startswith", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.ANY, VariableType.ANY }, VariableType.STRING));
            Objects.Add("text.getsubtext", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.ANY, VariableType.NUMBER, VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("text.getsubtexttoend", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.ANY, VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("text.converttolowercase", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.ANY }, VariableType.STRING));
            Objects.Add("text.converttouppercase", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.ANY }, VariableType.STRING));
            Objects.Add("text.getcharacter", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.STRING));
            Objects.Add("text.getcharactercode", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.ANY }, VariableType.NUMBER));
            Objects.Add("text.getindexof", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.ANY, VariableType.ANY }, VariableType.NUMBER));
            // THREAD
            Objects.Add("thread.yield", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("thread.createmutex", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("thread.lock", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("thread.unlock", new DefaultSignature(ObjectType.METHOD, 1, new List<VariableType> { VariableType.NUMBER }, VariableType.NON));
            Objects.Add("thread.run", new DefaultSignature(ObjectType.EVENT, 0, new List<VariableType>(), VariableType.NON));
            // TIME
            Objects.Add("time.get1", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset1", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get2", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset2", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get3", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset3", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get4", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset4", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get5", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset5", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get6", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset6", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get7", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset7", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get8", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset8", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            Objects.Add("time.get9", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NUMBER));
            Objects.Add("time.reset9", new DefaultSignature(ObjectType.METHOD, 0, new List<VariableType>(), VariableType.NON));
            // VECTOR
            Objects.Add("vector.init", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER }, VariableType.NUMBER_ARRAY));
            Objects.Add("vector.data", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.STRING }, VariableType.NUMBER_ARRAY));
            Objects.Add("vector.add", new DefaultSignature(ObjectType.METHOD, 3, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER_ARRAY, VariableType.NUMBER_ARRAY }, VariableType.NUMBER_ARRAY));
            Objects.Add("vector.sort", new DefaultSignature(ObjectType.METHOD, 2, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER_ARRAY }, VariableType.NUMBER_ARRAY));
            Objects.Add("vector.multiply", new DefaultSignature(ObjectType.METHOD, 5, new List<VariableType> { VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER, VariableType.NUMBER_ARRAY, VariableType.NUMBER_ARRAY }, VariableType.NUMBER_ARRAY));
        }

        internal static DefaultSignature Get(string key)
        {
            if (Objects.ContainsKey(key))
            {
                return Objects[key];
            }
            else
            {
                return new DefaultSignature(ObjectType.NON, 0, new List<VariableType>(), VariableType.NON);
            }
        }

        internal static VariableType GetOutputTupe(string name)
        {
            if (!Objects.ContainsKey(name))
            {
                return VariableType.NON;
            }
            else
            {
                return Objects[name].OutputType;
            }
        }

        internal static List<VariableType> GetInputsTupe(string name)
        {
            if (!Objects.ContainsKey(name))
            {
                return new List<VariableType>();
            }
            else
            {
                return Objects[name].InputType;
            }
        }

        internal static int GetInputCount(string name)
        {
            if (!Objects.ContainsKey(name))
            {
                return 0;
            }
            else
            {
                return Objects[name].InputCount;
            }
        }

        internal static ObjectType GetType(string name)
        {
            if (!Objects.ContainsKey(name))
            {
                return ObjectType.NON;
            }
            else
            {
                return Objects[name].Type;
            }
        }

        public static string ToString(string name)
        {
            if (!Objects.ContainsKey(name))
            {
                return "";
            }
            else
            {
                return Objects[name].ToString();
            }
        }
    }
}
