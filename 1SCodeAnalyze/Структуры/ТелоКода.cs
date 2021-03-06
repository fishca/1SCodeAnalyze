﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _1SCodeAnalyze.Структуры
{

	/// <summary>
	/// Тело кода. класс куда попадает и анализируется тело процедуры или цикла
	/// </summary>
    class ТелоКода
    {
        String Текст;
        int inIndex;
        private Boolean АнализПрямогоЗапросаПроведен;
        private Boolean ЗапросЕсть;
        private ИнформацияАнализа Анализ;

        public ТелоКода(String Текст,  int inIndex) {
            ЗапросЕсть = false;
            this.inIndex = inIndex;
            this.Текст = Текст;
            АнализПрямогоЗапросаПроведен = false;
        }

        private ИнформацияАнализа ПрямойЗапрос()
        {
            var ПоискЗапроса = new Regex(@"^[^\/\n]*?\.(выполнить|найтипокоду|найтипореквизиту|найтипонаименованию|FindByCode|FindByDescription|FindByAttribute|Execute)[\s]?\([^\n]*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match Найдены = ПоискЗапроса.Match(Текст);
            if (!Найдены.Success)
                return null;
			String КусокКода = " " + (Текст.Length > 20 ? Текст.Substring(0, 20).Trim() : "") + "\n...\n" + Найдены.Value.Trim();
			if(Найдены.Index < 20)КусокКода = (Текст.Length > 40 ? Текст.Substring(0, 40).Trim() : Найдены.Value.Trim());
			return new ИнформацияАнализа(Найдены.Index + inIndex, КусокКода, Найдены.Groups[1].Value);
        }

        public ТелоКода ПровестиАнализ()
        {
            if (АнализПрямогоЗапросаПроведен) return this;
            Анализ = ПрямойЗапрос();
            ЗапросЕсть = (Анализ != null);
            АнализПрямогоЗапросаПроведен = true;
            return this;
        }

        public ИнформацияАнализа ПолучитьАнализ(){
            ПровестиАнализ();
            return Анализ;
        }


        public Boolean ЕстьЗапрос()
        {
            ПровестиАнализ();
            return ЗапросЕсть;
        }

        public MatchCollection НайтиВызовы()
        {
            var ПоискВызовов = new Regex(@"^[^\.\/\n]*?([а-яa-z0-9_]*?)[\s]?\(", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return ПоискВызовов.Matches(Текст);
        }
    }
}
