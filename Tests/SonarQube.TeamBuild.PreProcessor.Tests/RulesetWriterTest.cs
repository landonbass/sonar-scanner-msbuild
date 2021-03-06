﻿/*
 * SonarQube Scanner for MSBuild
 * Copyright (C) 2015-2017 SonarSource SA and Microsoft Corporation
 * mailto: contact AT sonarsource DOT com
 *
 * Licensed under the MIT License.
 * See LICENSE file in the project root for full license information.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace SonarQube.TeamBuild.PreProcessor.UnitTests
{
    [TestClass]
    public class RulesetWriterTest
    {
        [TestMethod]
        public void RulesetWriterShouldFailToTwiceSeveralTimesIdenticalCheckId()
        {
            List<string> ids = new List<string>();
            ids.Add("CA1000");
            ids.Add("CA1000");
            ids.Add("CA1001");
            ids.Add("CA1002");
            ids.Add("CA1002");
            ids.Add("CA1002");

            try
            {
                RulesetWriter.ToString(ids);
            }
            catch (ArgumentException e)
            {
                if ("The following CheckId should not appear multiple times: CA1000, CA1002".Equals(e.Message))
                {
                    return;
                }
            }

            Assert.Fail();
        }

        [TestMethod]
        public void RulesetWriterToString()
        {
            List<string> ids = new List<string>();
            ids.Add("CA1000");
            ids.Add("MyCustomCheckId");

            string actual = RulesetWriter.ToString(ids);

            StringBuilder expected = new StringBuilder();
            expected.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            expected.AppendLine("<RuleSet Name=\"SonarQube\" Description=\"Rule set generated by SonarQube\" ToolsVersion=\"12.0\">");
            expected.AppendLine("  <Rules AnalyzerId=\"Microsoft.Analyzers.ManagedCodeAnalysis\" RuleNamespace=\"Microsoft.Rules.Managed\">");
            expected.AppendLine("    <Rule Id=\"CA1000\" Action=\"Warning\" />");
            expected.AppendLine("    <Rule Id=\"MyCustomCheckId\" Action=\"Warning\" />");
            expected.AppendLine("  </Rules>");
            expected.AppendLine("</RuleSet>");

            Assert.AreEqual(expected.ToString(), actual);
        }
    }
}
