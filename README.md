# 📅 AgendaApp - Sistema de Agendamentos  

## 🚀 **Visão Geral**  
Aplicação desktop completa para **gestão profissional de compromissos**, desenvolvida com:  
✔ Autenticação segura de usuários  
✔ CRUD completo de agendamentos  
✔ Interface moderna e intuitiva  
✔ Validações em tempo real  

**Destaques técnicos que impressionam**:  
🔐 **Segurança avançada** com hash SHA256  
📊 **Arquitetura limpa** seguindo Clean Code  
⚡ **Performance otimizada** com Entity Framework  

---

## 🛠 **Tecnologias Utilizadas**  

| Categoria         | Tecnologias                          |
|-------------------|--------------------------------------|
| **Linguagem**     | VB.NET                               |
| **Framework**     | .Net Framework 4.6.1                 |
| **Interface**     | Windows Forms                        |
| **Banco de Dados**| SQL Server + Entity Framework       |
| **Padrões**       | Repository Pattern, SOLID           |
| **Segurança**     | Hash SHA256, Validação Multi-camada |

---

## 🔧 **Configuração Rápida**  

1️⃣ **Banco de Dados** (2 opções):  

**Opção 1 - App.config** (para testes):  
```xml
<connectionStrings>
	<add name="AgendaContext"
		 connectionString="Data Source=URL_BANCO;Initial Catalog=AgendaDB;User ID=USER;Password=SENHA;MultipleActiveResultSets=True"
		 providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Opção 2 - User Secrets** (recomendado):  
```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:AgendaContext" "SuaStringAqui"
```

2️⃣ **Executando:**

```bash
# No Visual Studio:
1. Abra a solução
2. Pressione F5
```

## ✨ Diferenciais Competitivos
**✅ Sistema de Login Inteligente**
- Validação em tempo real de credenciais
- Feedback visual intuitivo

**✅ Gestão de Compromissos**

- Pesquisa por datas
- Notificações de conflitos
- Interface responsiva

**✅ Arquitetura Profissional**

- Separação clara de camadas
- Fácil manutenção e expansão

## 📌 Próximos Passos (Roadmap)
- Integração com calendário Google
- Versão mobile complementar
- Sistema de lembretes por e-mail


## 🤝 Contribuições

Sinta-se à vontade para contribuir com este projeto! Sugestões de melhorias, criação de issues e pull requests são bem-vindas.

## 📧 Contato

-   Email: hnriq.donha@gmail.com
-   LinkedIn: https://www.linkedin.com/in/pedro-donha/
